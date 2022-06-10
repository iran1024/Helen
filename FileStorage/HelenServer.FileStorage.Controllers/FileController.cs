using HelenServer.FileStorage.Contracts;

namespace HelenServer.FileStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileStorageService _service;

        public FileController(IFileStorageService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<OperationResult> UploadFileAsync()
        {
            if (Request.Form.Files.Count < 1)
            {
                return OperationResult.Error(new FileNotFoundException());
            }
            else
            {
                var file = Request.Form.Files[0];

                using var fileStream = file.OpenReadStream();

                var model = new UploadModel
                {
                    GroupName = "group1",
                    ClusterName = "",
                    FileName = file.FileName,
                    FileExt = Path.GetExtension(file.FileName),
                    FileStream = fileStream
                };

                var operation = HttpContext.GetOperation(model);

                var result = await _service.UploadFileAsync(operation, HttpContext.RequestAborted);

                return result;
            }
        }
    }
}