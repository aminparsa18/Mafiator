using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Data;
using Mafiator.Service.Contracts;
using Mafiator.Service.Contracts.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

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
            user.FirstOrDefault().Image = Data.Constants.BlobStorageEndpoint + user.FirstOrDefault().Image;
            return Ok(new ApiResult<ValidateUserResult>()
            {
                IsSuccess = true,
                Data = user.FirstOrDefault()
            });
        }

        [HttpPost]
        public IActionResult SendMessage()
        {
            smsSender.SendAuthSmsAsync("52005", "+905316335119");
            return Ok();
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
                    {"access_token", auth.Properties.GetTokenValue("access_token")},
                    {"refresh_token", auth.Properties.GetTokenValue("refresh_token") ?? string.Empty},
                    {"expires", (auth.Properties.ExpiresUtc?.ToUnixTimeSeconds() ?? -1).ToString()},
                    {"email", email}
                };
                // Build the result url
                var url = "kirekhar://#" + string.Join(
                    "&",
                    qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
                        .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

                // Redirect to final url
                Request.HttpContext.Response.Redirect(url);
            }
        }
    }
}