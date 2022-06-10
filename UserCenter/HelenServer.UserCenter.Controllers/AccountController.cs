using HelenServer.Core;
using HelenServer.UserCenter.Contracts;
using HelenServer.UserCenter.Shared;
using Microsoft.AspNetCore.Mvc;

namespace HelenServer.UserCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("verify")]
        public Task<OperationResult> SendVerifyCodeAsync([FromBody] RegisterModel model)
        {
            return _service.SendVerifyCodeAsync(model, HttpContext.RequestAborted);
        }

        [HttpPost]
        public Task<OperationResult> RegistAsync([FromBody] RegisterModel model)
        {
            return _service.RegistAsync(model, HttpContext.RequestAborted);
        }

        [HttpPut]
        public Task<OperationResult> UpdatePasswordAsync([FromBody] RegisterModel model)
        {
            return _service.UpdatePasswordAsync(model, HttpContext.RequestAborted);
        }

        [HttpGet]
        public Task<OperationResult> ExistsAccountAsync(string account)
        {
            return _service.ExistsAccountAsync(account, HttpContext.RequestAborted);
        }
    }
}
