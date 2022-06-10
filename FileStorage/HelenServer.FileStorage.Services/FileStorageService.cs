using HelenServer.FileStorage.Contracts;

namespace HelenServer.FileStorage.Services
{
    [Injection(typeof(IFileStorageService))]
    public class FileStorageService : IFileStorageService
    {
        private readonly IDalFileStorageService _service;

        public FileStorageService(IDalFileStorageService service)
        {
            _service = service;
        }

        public ValueTask<OperationResult<byte[]>> DownloadFileAsync(Operation<FileInfoModel> operation)
        {
            return _service.DownloadFileAsync(operation);
        }

        public ValueTask<OperationResult<FastDFSFileInfoModel>> GetFileInfo(Operation<FileInfoModel> operation)
        {
            return _service.GetFileInfo(operation);
        }

        public ValueTask<OperationResult<StorageNodeModel>> GetStorageNodeAsync(Operation<FileGroupModel> operation)
        {
            return _service.GetStorageNodeAsync(operation);
        }

        public OperationResult<string> GetToken(Operation<FilePlaceModel> operation, DateTime? dateTime = null)
        {
            return _service.GetToken(operation, dateTime);
        }

        public ValueTask<IReadOnlyCollection<GroupInfoModel>> ListAllGroupInfosAsync(Operation<string> operation)
        {
            return _service.ListAllGroupInfosAsync(operation);
        }

        public ValueTask<OperationResult<GroupInfoModel>> ListOneGroupInfoAsync(Operation<FileGroupModel> operation)
        {
            return _service.ListOneGroupInfoAsync(operation);
        }

        public ValueTask<IReadOnlyCollection<StorageInfoModel>> ListStorageInfosAsync(Operation<FileGroupModel> operation)
        {
            return _service.ListStorageInfosAsync(operation);
        }

        public ValueTask<OperationResult<StorageNodeModel>> QueryStorageNodeForFileAsync(Operation<FileInfoModel> operation)
        {
            return _service.QueryStorageNodeForFileAsync(operation);
        }

        public ValueTask<IReadOnlyCollection<StorageNodeModel>> QueryStorageNodesForFileAsync(Operation<FileInfoModel> operation)
        {
            return _service.QueryStorageNodesForFileAsync(operation);
        }

        public ValueTask<OperationResult<bool>> RemoveFileAsync(Operation<FileInfoModel> operation)
        {
            return _service.RemoveFileAsync(operation);
        }

        public ValueTask<OperationResult<string>> UploadFileAsync(Operation<UploadModel> operation, CancellationToken cancellationToken = default)
        {
            return _service.UploadFileAsync(operation, cancellationToken);
        }
    }
}