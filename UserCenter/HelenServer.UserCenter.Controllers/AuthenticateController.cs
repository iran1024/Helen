using HelenServer.Authentication.OAuth2;
using HelenServer.Core;
using HelenServer.UserCenter.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HelenServer.UserCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public Task<OperationResult<LoginResult>> LoginAsync([FromBody] LoginModel loginModel)
        {
            return _authenticationService.LoginAsync(loginModel, HttpContext.RequestAborted);
        }

        [HttpGet]
        public Task<OperationResult<LogoutModel>> LogoutAsync(string logoutId)
        {
            var model = new LogoutModel
            {
                LogoutId = logoutId,
                ShowLogoutPrompt = true
            };

            return _authenticationService.LogoutAsync(model, HttpContext.RequestAborted);
        }

        [HttpPost("logout")]
        public Task<OperationResult<LogoutModel>> LogoutAsync([FromBody] LogoutInputModel model)
        {
            var loggedOutModel = new LogoutModel
            {
                LogoutId = model.LogoutId,
                ShowLogoutPrompt = false
            };

            return _authenticationService.LogoutAsync(loggedOutModel, HttpContext.RequestAborted);
        }
    }
}