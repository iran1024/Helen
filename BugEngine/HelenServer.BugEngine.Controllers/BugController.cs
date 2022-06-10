using HelenServer.BugEngine.Contracts;
using HelenServer.Core;
using Microsoft.AspNetCore.Mvc;

namespace HelenServer.BugEngine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize("IdentityServerAccessToken")]
    public class BugController : ControllerBase
    {
        private readonly IBugService _service;

        public BugController(IBugService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<BugModel>> GetAll()
        {
            return await _service.GetAllAsync(HttpContext.RequestAborted);
        }

        [HttpPost]
        public async Task<OperationResult> InsertAsync([FromBody] BugModel model)
        {
            var operation = HttpContext.GetOperation(model);

            return await _service.InsertAsync(operation, HttpContext.RequestAborted);
        }

        [HttpPut]
        public async Task<OperationResult> UpdateAsync([FromBody] BugModel model)
        {
            var operation = HttpContext.GetOperation(model);

            return await _service.UpdateAsync(operation, HttpContext.RequestAborted);
        }

        [HttpDelete]
        public async Task<OperationResult> DeleteAsync(int id)
        {
            var operation = HttpContext.GetOperation(id);

            return await _service.DeleteAsync(operation, HttpContext.RequestAborted);
        }
    }
}