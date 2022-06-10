using HelenServer.Core;
using HelenServer.UserCenter.Contracts;
using HelenServer.UserCenter.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelenServer.UserCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("IdentityServerAccessToken")]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _service;

        public UserController(IAccountService service)
        {
            _service = service;
        }

        [HttpPut]
        public Task<OperationResult> UpdateAsync([FromBody] UserModel userModel)
        {
            var operation = HttpContext.GetOperation(userModel);

            return _service.UpdateAsync(operation, HttpContext.RequestAborted);
        }

        [HttpPut]
        public Task<OperationResult> UpdateStatusAsync([FromBody] UserStatusModel model)
        {
            var operation = HttpContext.GetOperation(model);

            return _service.UpdateStatusAsync(operation, HttpContext.RequestAborted);
        }

        [HttpDelete]
        public Task<OperationResult> DeleteAsync(string userId)
        {
            var operation = HttpContext.GetOperation(userId);

            return _service.DeleteAsync(operation, HttpContext.RequestAborted);
        }

        [HttpGet]
        public Task<OperationResult<UserModel>> GetAsync()
        {
            var operation = HttpContext.GetUser();

            return _service.GetAsync(operation, HttpContext.RequestAborted);
        }

        [HttpGet]
        public Task<OperationResult<UserModel>> GetByUserIdAsync([FromQuery] string userId)
        {
            var operation = new OperationUser(userId, string.Empty, Array.Empty<string>(), false);

            return _service.GetAsync(operation, HttpContext.RequestAborted);
        }

        [HttpGet]
        public Task<IReadOnlyCollection<UserModel>> GetListAsync([FromQuery] UserSearchModel model)
        {
            var operation = HttpContext.GetOperation(model);

            return _service.GetListAsync(operation, HttpContext.RequestAborted);
        }
    }
}
