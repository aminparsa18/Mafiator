using AutoMapper;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Common.Helpers;
using Mafiator.Data;
using Mafiator.Entities.Identity;
using Mafiator.Service.Contracts.Users;
using Microsoft.AspNetCore.Identity;
using RepoDb;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Users;

public class UserRegisterService : IUserRegisterService
{
    private readonly IDbConnection _dbConnection;
    private readonly IMapper _mapper;
    private readonly IValidator<RegisterUserRequest> _validator;
    private readonly UserManager<User> _userManager;

    public UserRegisterService(IDbConnection dbConnection, IMapper mapper, IValidator<RegisterUserRequest> validator, UserManager<User> userManager)
    {
        _dbConnection = dbConnection;
        _mapper = mapper;
        _validator = validator;
        _userManager = userManager;
    }

    public async Task<ApiResult> Register(RegisterUserRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            };
        var users = await _dbConnection.ExecuteQueryAsync<User>(
           "SELECT TOP 1 * FROM [Users] WHERE Username = @username", new { username = request.Username });
        if (users.Any())
        {
            return new ApiResult()
            {
                StatusCode = ApiResultStatusCode.Conflict,
                Errors = new[] { "User already exist." }
            };
        }

        var user = _mapper.Map<RegisterUserRequest, User>(request);
        user.Id = Guid.NewGuid(); ;
        user.Code = RandomHelper.CreateRandomText(10);
        user.Score = 100;
        var createdUser = await _userManager.CreateAsync(user, request.Password);
        if (!createdUser.Succeeded)
        {
            return new ApiResult()
            {
                StatusCode = ApiResultStatusCode.LogicError,
                Errors = createdUser.Errors.Select(x => x.Description)
            };
        }

        await _userManager.AddToRoleAsync(user, Constants.PlayerRole);
        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
        //smsSender.SendAuthSmsAsync(token, user.PhoneNumber);
        return new ApiResult()
        {
            IsSuccess = true
        };
    }
}