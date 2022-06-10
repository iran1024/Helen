using HelenServer.BugEngine.Contracts;

namespace HelenServer.BugEngine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MigrateController : ControllerBase
    {
        private readonly IMigrateService _service;

        public MigrateController(IMigrateService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<OperationResult> MigrateAsync()
        {
            var file = Request.Form.Files[0];

            if (file is null)
            {
                return OperationResult.Failed("请上传禅道导出文件");
            }

            using var stream = new MemoryStream();

            await file.CopyToAsync(stream, HttpContext.RequestAborted);

            var xml = Encoding.UTF8.GetString(stream.GetBuffer());

            if (await _service.MigrateAsync(xml, HttpContext.RequestAborted))
            {
                return OperationResult.OK("迁移成功");
            }

            return OperationResult.Failed("迁移失败");
        }
    }
}