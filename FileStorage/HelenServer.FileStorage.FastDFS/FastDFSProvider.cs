using AutoMapper;
using FastDFSCore;
using FastDFSCore.Protocols;
using HelenServer.FileStorage.Contracts;

namespace HelenServer.FileStorage.FastDFS
{
    [Injection(typeof(IFileStorageProvider))]
    public class FastDFSProvider : IFileStorageProvider
    {
        private readonly IFastDFSClient _client;
        private readonly IMapper _mapper;

        public FastDFSProvider(IFastDFSClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async ValueTask<byte[]> DownloadFileAsync(FileInfoModel model)
        {
            var storageNode = await _client.GetStorageNodeAsync(model.GroupName, model.ClusterName);

            return await _client.DownloadFileAsync(storageNode, model.FileId, model.ClusterName);
        }

        public async ValueTask<FastDFSFileInfoModel> GetFileInfo(FileInfoModel model)
        {
            var storageNode = await _client.GetStorageNodeAsync(model.GroupName, model.ClusterName);

            var result = await _client.GetFileInfo(storageNode, model.FileId, model.ClusterName);

            return _mapper.Map<FastDFSFileInfo, FastDFSFileInfoModel>(result);
        }

        public async ValueTask<StorageNodeModel> GetStorageNodeAsync(FileGroupModel model)
        {
            var result = await _client.GetStorageNodeAsync(model.GroupName, model.ClusterName);

            return _mapper.Map<StorageNode, StorageNodeModel>(result);
        }

        public string GetToken(FilePlaceModel model, DateTime? dateTime = null)
        {
            return _client.GetToken(model.FileId, dateTime, model.ClusterName);
        }

        public async ValueTask<IReadOnlyCollection<GroupInfoModel>> ListAllGroupInfosAsync(string clusterName)
        {
            var result = await _client.ListAllGroupInfosAsync(clusterName);

            return _mapper.Map<List<GroupInfo>, List<GroupInfoModel>>(result).AsReadOnly();
        }

        public async ValueTask<GroupInfoModel> ListOneGroupInfoAsync(FileGroupModel model)
        {
            var result = await _client.ListOneGroupInfoAsync(model.GroupName, model.ClusterName);

            return _mapper.Map<GroupInfo, GroupInfoModel>(result);
        }

        public async ValueTask<IReadOnlyCollection<StorageInfoModel>> ListStorageInfosAsync(FileGroupModel model)
        {
            var result = await _client.ListStorageInfosAsync(model.GroupName, model.ClusterName);

            return _mapper.Map<List<StorageInfo>, List<StorageInfoModel>>(result).AsReadOnly();
        }

        public async ValueTask<StorageNodeModel> QueryStorageNodeForFileAsync(FileInfoModel model)
        {
            var result = await _client.QueryStorageNodeForFileAsync(model.GroupName, model.FileId, model.ClusterName);

            return _mapper.Map<StorageNode, StorageNodeModel>(result);
        }

        public async ValueTask<IReadOnlyCollection<StorageNodeModel>> QueryStorageNodesForFileAsync(FileInfoModel model)
        {
            var result = await _client.QueryStorageNodesForFileAsync(model.GroupName, model.FileId, model.ClusterName);

            return _mapper.Map<List<StorageNode>, List<StorageNodeModel>>(result).AsReadOnly();
        }

        public async ValueTask<bool> RemoveFileAsync(FileInfoModel model)
        {
            var result = await _client.RemoveFileAsync(model.GroupName, model.FileId, model.ClusterName);

            return result;
        }

        public async ValueTask<string> UploadFileAsync(UploadModel model)
        {
            var storageNode = await _client.GetStorageNodeAsync(model.GroupName, model.ClusterName);

            var result = await _client.UploadFileAsync(storageNode, model.FileStream, model.FileExt, model.ClusterName);

            return result;
        }
    }
}