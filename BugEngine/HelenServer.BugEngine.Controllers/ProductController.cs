using HelenServer.BugEngine.Contracts;
using HelenServer.Core;
using Microsoft.AspNetCore.Mvc;

namespace HelenServer.BugEngine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync(HttpContext.RequestAborted));
        }

        [HttpPost]
        public async Task<IActionResult> InsertAsync()
        {
            return Ok(await _service.InsertAsync(HttpContext.RequestAborted));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync()
        {
            return Ok(await _service.UpdateAsync(null, HttpContext.RequestAborted));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {
            var operation = HttpContext.GetOperation("5");

            return Ok(await _service.DeleteAsync(operation, HttpContext.RequestAborted));
        }
    }
}