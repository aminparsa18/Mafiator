using AutoMapper;
using Mafiator.Common.Api;
using Mafiator.Common.Helpers;
using Mafiator.Data;
using Mafiator.Data.Dtos.User;
using Mafiator.Entities;
using Mafiator.Entities.Identity;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Impl.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IMapper mapper;
        protected readonly IDbConnection dbConnection;
        private readonly ISmsSender smsSender;
        private readonly ITokenService tokenService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IUnitOfWork unitOfWork;

        public IdentityService(IMapper mapper, IDbConnection dbConnection, ISmsSender smsSender,
            ITokenService tokenService,
            UserManager<User> userManager, RoleManager<Role> roleManager, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.dbConnection = dbConnection;
            this.tokenService = tokenService;
            this.smsSender = smsSender;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ValidateUserDto>> GetByUsername(string username)
        {
            return await dbConnection.ExecuteQueryAsync<ValidateUserDto>(
                "SELECT TOP 1 [Id],[DisplayName],[Image] FROM [Users] WHERE Username = @username", new {username});
        }

        public async Task<AuthResult> Login(UserLoginDto userLogin)
        {
            var users = await dbConnection.ExecuteQueryAsync<User>(
                "SELECT TOP 1 * FROM [Users] WHERE Username = @username", new {username = userLogin.Username});
            if (!users.Any())
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.UnAuthorized,
                    Errors = new[] {"Login data is not correct."}
                };
            }

            var user = users.FirstOrDefault();
            var userHasValidPassword = await userManager.CheckPasswordAsync(user, userLogin.Password);
            if (!userHasValidPassword)
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.UnAuthorized,
                    Errors = new[] {"Login data is not correct."}
                };
            }

            if (!user.PhoneNumberConfirmed)
            {
                var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
                smsSender.SendAuthSmsAsync(token, user.PhoneNumber);
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.Forbidden,
                    Errors = new[] {"Phone number is not confirmed"},
                    Token = user.PhoneNumber
                };
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, Constants.PlayerRole),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, user.Id.ToString())
            };
            var tokenResult = tokenService.GenerateAccessToken(user, claims);
            var refreshToken = new RefreshToken()
            {
                JwtId = tokenResult.JwtId,
                UserId = user.Id,
                ExpirationDate = DateTime.UtcNow.AddMonths(6),
                Token = tokenService.GenerateRefreshToken()
            };
            await unitOfWork.RefreshToken.AddFast(refreshToken);
            return new AuthResult()
            {
                IsSuccess = true,
                Token = tokenResult.Token,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<ApiResult> Register(RegisterUserDto registerUser)
        {
            var users = await dbConnection.ExecuteQueryAsync<User>(
                "SELECT TOP 1 * FROM [Users] WHERE Username = @username", new {username = registerUser.Username});
            if (users.Any())
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.Conflict,
                    Errors = new[] {"User already exist."}
                };
            }

            var user = mapper.Map<RegisterUserDto, User>(registerUser);
            user.Id = Guid.NewGuid();;
            user.Code = RandomHelper.RandomStr(10);
            user.Score = 100;
            var createdUser = await userManager.CreateAsync(user, registerUser.Password);
            if (!createdUser.Succeeded)
            {
                return new ApiResult()
                {
                    StatusCode = ApiResultStatusCode.LogicError,
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            await userManager.AddToRoleAsync(user, Constants.PlayerRole);
            var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            smsSender.SendAuthSmsAsync(token, user.PhoneNumber);
            return new ApiResult()
            {
                IsSuccess = true
            };
        }

        public async Task<AuthResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            //pass refresh token
            var principle = tokenService.GetPrincipalFromExpiredToken(refreshTokenRequest.Token);
            if (principle == null)
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.BadRequest,
                    Errors = new[] {"Invalid Token"}
                };
            }

            var storedRefreshTokens = await unitOfWork.RefreshToken.GetByToken(refreshTokenRequest.RefreshToken);
            if (!storedRefreshTokens.Any())
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.NotFound,
                    Errors = new[] {"Refresh Token does not exist"}
                };
            }

            var storedRefreshToken = storedRefreshTokens.FirstOrDefault();
            if (DateTime.UtcNow > storedRefreshToken.ExpirationDate)
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.LogicError,
                    Errors = new[] {"Refresh Token has expired"}
                };
            }

            if (storedRefreshToken.IsInvalidated)
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.BadRequest,
                    Errors = new[] {"Refresh Token Invalidated"}
                };
            }

            if (storedRefreshToken.IsUsed)
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.BadRequest,
                    Errors = new[] {"This refresh token has been used"}
                };
            }

            var jti = principle.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.BadRequest,
                    Errors = new[] {"This refresh token does not match this JWT"}
                };
            }

            await unitOfWork.RefreshToken.SetUsed(storedRefreshToken.Id);
            var user = await userManager.FindByIdAsync(principle.FindFirstValue(ClaimTypes.Name));
            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, Constants.PlayerRole),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, user.Id.ToString())
            };
            var tokenResult = tokenService.GenerateAccessToken(user, claims);
            var refreshToken = new RefreshToken()
            {
                JwtId = tokenResult.JwtId,
                UserId = user.Id,
                ExpirationDate = DateTime.UtcNow.AddMonths(6),
                Token = tokenService.GenerateRefreshToken()
            };
            await unitOfWork.RefreshToken.AddFast(refreshToken);
            return new AuthResult()
            {
                IsSuccess = true,
                Token = tokenResult.Token,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResult> ConfirmPhoneNumber(string phoneNo, string token)
        {
            var existingUser = await userManager.Users.FirstOrDefaultAsync(e => e.PhoneNumber == phoneNo);
            if (existingUser == null)
            {
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.UnAuthorized,
                    Errors = new[] {"User does not exist"}
                };
            }

            var result = await userManager.ChangePhoneNumberAsync(existingUser, phoneNo, token);
            if (!result.Succeeded)
                return new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.BadRequest,
                    Errors = result.Errors.Select(s => s.Description)
                };

            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, Constants.PlayerRole),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Name, existingUser.Id.ToString())
            };

            var userClaims = await userManager.GetClaimsAsync(existingUser);
            claims.AddRange(userClaims);
            var userRoles = await userManager.GetRolesAsync(existingUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;
                    claims.Add(roleClaim);
                }
            }

            var tokenResult = tokenService.GenerateAccessToken(existingUser, claims);
            var refreshToken = new RefreshToken()
            {
                JwtId = tokenResult.JwtId,
                UserId = existingUser.Id,
                ExpirationDate = DateTime.UtcNow.AddMonths(6),
                Token = tokenService.GenerateRefreshToken()
            };
            await unitOfWork.RefreshToken.AddFast(refreshToken);
            existingUser.PhoneNumberConfirmed = true;
            await userManager.UpdateAsync(existingUser);
            return new AuthResult()
            {
                IsSuccess = true,
                Token = tokenResult.Token,
                RefreshToken = refreshToken.Token
            };

        }

        public async Task<ApiResult> UpdateProfile(string userId, string name, string image)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResult()
                {
                    StatusCode = ApiResultStatusCode.UnAuthorized,
                    Errors = new[] {"User does not exist"}
                };
            }

            user.DisplayName = name;
            user.Image = image;
            await userManager.UpdateAsync(user);
            return new ApiResult() {IsSuccess = true};
        }

        public async Task<ApiResult<UserDto>> GetUser(string userId)
        {
            var users = await dbConnection.ExecuteQueryAsync<UserDto>(
                "SELECT TOP 1 [Image],[Score],[DisplayName],[CountryCode] FROM [Users] WHERE Id = @id",
                new {id = userId});
            if (!users.Any())
            {
                return new ApiResult<UserDto>()
                {
                    StatusCode = ApiResultStatusCode.UnAuthorized,
                    Errors = new[] {"User does not exist"}
                };
            }

            return new ApiResult<UserDto>()
            {
                IsSuccess = true,
                Data = users.FirstOrDefault()
            };
        }
    }
}