namespace HelenServer.FileStorage.Contracts;

public interface IFileStorageProvider
{
    ValueTask<byte[]> DownloadFileAsync(FileInfoModel model);

    ValueTask<FastDFSFileInfoModel> GetFileInfo(FileInfoModel model);

    ValueTask<StorageNodeModel> GetStorageNodeAsync(FileGroupModel model);

    string GetToken(FilePlaceModel model, DateTime? dateTime = null);

    ValueTask<IReadOnlyCollection<GroupInfoModel>> ListAllGroupInfosAsync(string clusterName);

    ValueTask<GroupInfoModel> ListOneGroupInfoAsync(FileGroupModel model);

    ValueTask<IReadOnlyCollection<StorageInfoModel>> ListStorageInfosAsync(FileGroupModel model);

    ValueTask<StorageNodeModel> QueryStorageNodeForFileAsync(FileInfoModel model);

    ValueTask<IReadOnlyCollection<StorageNodeModel>> QueryStorageNodesForFileAsync(FileInfoModel model);

    ValueTask<bool> RemoveFileAsync(FileInfoModel model);

    ValueTask<string> UploadFileAsync(UploadModel model);
}