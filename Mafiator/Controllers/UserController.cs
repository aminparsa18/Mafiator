using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Service.Contracts;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers
{
    public class UserController : ApiBaseController
    {
        private readonly IIdentityService identityService;
        private readonly ISmsSender smsSender;

        public UserController(IIdentityService identityService, ISmsSender smsSender)
        {
            this.identityService = identityService;
            this.smsSender = smsSender;
        }

        [HttpGet]
        public async Task<IActionResult> ValidateUser(string username)
        {
            var user = await identityService.GetByUsername(username);
            if (!user.Any())
                return Ok(new ApiResult()
                {
                    IsSuccess = false,
                    StatusCode = ApiResultStatusCode.NotFound,
                    Errors = new[] {"User not Found"}
                });
           // user.ForAll(u=>u.Image=Constants.BlobStorageEndpoint+u.Image);
            return Ok(new ApiResult<ValidateUserDto>()
            {
                IsSuccess = true,
                Data = user.FirstOrDefault()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] string name)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var res = await identityService.UpdateProfilePicture(userId, name);
            if (res.IsSuccess)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost]
        public IActionResult SendMessage()
        {
            smsSender.SendAuthSmsAsync("52005", "+989919002102");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPhoneNo([FromBody] ConfirmPhoneDto confirmPhoneDto)
        {
            if (!ModelState.IsValid)
                return Ok(new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.BadRequest,
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(s => s.ErrorMessage))
                });
            var res = await identityService.ConfirmPhoneNumber(confirmPhoneDto.PhoneNo, confirmPhoneDto.Token);
            if (res.IsSuccess)
                return Ok(res);
            return BadRequest(res);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return Ok(new AuthResult()
                {
                    StatusCode = ApiResultStatusCode.BadRequest,
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(s => s.ErrorMessage))
                });
            var result = await identityService.Login(loginDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ApiResult()
                {
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(s => s.ErrorMessage)),
                    StatusCode = ApiResultStatusCode.BadRequest
                });
            }

            var result = await identityService.Register(userDto);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var result = await identityService.RefreshToken(refreshTokenRequest);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var res = await identityService.GetUser(userId);
            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var res = await identityService.GetUser(userId);
            return Ok(res);
        }

        [HttpGet]
        public async Task GoogleLogin()
        {
            var auth = await Request.HttpContext.AuthenticateAsync("Google");

            if (!auth.Succeeded
                || auth?.Principal == null
                || !auth.Principal.Identities.Any(id => id.IsAuthenticated)
                || string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
            {
                // Not authenticated, challenge
                await Request.HttpContext.ChallengeAsync("Google");
            }
            else
            {
                var claims = auth.Principal.Identities.FirstOrDefault()?.Claims;
                var email = string.Empty;
                email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                // Get parameters to send back to the callback
                var qs = new Dictionary<string, string>
                {
                    { "access_token", auth.Properties.GetTokenValue("access_token") },
                    { "refresh_token", auth.Properties.GetTokenValue("refresh_token") ?? string.Empty },
                    { "expires", (auth.Properties.ExpiresUtc?.ToUnixTimeSeconds() ?? -1).ToString() },
                    { "email", email }
                };
                // Build the result url
                var url =  "kirekhar://#" + string.Join(
                    "&",
                    qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
                        .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

                // Redirect to final url
                Request.HttpContext.Response.Redirect(url);
            }
        }
    }
}