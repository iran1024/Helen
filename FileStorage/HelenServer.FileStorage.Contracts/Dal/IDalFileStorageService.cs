namespace HelenServer.FileStorage.Contracts
{
    public interface IDalFileStorageService
    {
        ValueTask<OperationResult<byte[]>> DownloadFileAsync(Operation<FileInfoModel> operation);

        ValueTask<OperationResult<FastDFSFileInfoModel>> GetFileInfo(Operation<FileInfoModel> operation);

        ValueTask<OperationResult<StorageNodeModel>> GetStorageNodeAsync(Operation<FileGroupModel> operation);

        OperationResult<string> GetToken(Operation<FilePlaceModel> operation, DateTime? dateTime = null);

        ValueTask<IReadOnlyCollection<GroupInfoModel>> ListAllGroupInfosAsync(Operation<string> operation);

        ValueTask<OperationResult<GroupInfoModel>> ListOneGroupInfoAsync(Operation<FileGroupModel> operation);

        ValueTask<IReadOnlyCollection<StorageInfoModel>> ListStorageInfosAsync(Operation<FileGroupModel> operation);

        ValueTask<OperationResult<StorageNodeModel>> QueryStorageNodeForFileAsync(Operation<FileInfoModel> operation);

        ValueTask<IReadOnlyCollection<StorageNodeModel>> QueryStorageNodesForFileAsync(Operation<FileInfoModel> operation);

        ValueTask<OperationResult<bool>> RemoveFileAsync(Operation<FileInfoModel> operation);

        ValueTask<OperationResult<string>> UploadFileAsync(Operation<UploadModel> operation, CancellationToken cancellationToken = default);
    }
}