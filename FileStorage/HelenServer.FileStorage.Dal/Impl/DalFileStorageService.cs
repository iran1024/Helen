using HelenServer.Common;
using HelenServer.FileStorage.Contracts;

namespace HelenServer.FileStorage.Dal
{
    [ServiceProvider(typeof(IDalFileStorageService), SqlServerConstants.ProviderName)]
    public class DalFileStorageService : IDalFileStorageService
    {
        private readonly IRepo<AttachmentDbContext> _repo;
        private readonly IServiceProvider _provider;
        private readonly IXXH64 _xxh;

        public DalFileStorageService(IReadWriteSplittingDbContextFactory<AttachmentDbContext> factory, IServiceProvider provider, IXXH64 xxh)
        {
            _repo = new Repo<AttachmentDbContext>(factory.Create(ReadWritePolicy.ReadWrite));
            _provider = provider;
            _xxh = xxh;
        }

        public async ValueTask<OperationResult<byte[]>> DownloadFileAsync(Operation<FileInfoModel> operation)
        {
            var fs = this.GetProvider();

            return OperationResult<byte[]>.OK(await fs.DownloadFileAsync(operation.Parameter));
        }

        public async ValueTask<OperationResult<FastDFSFileInfoModel>> GetFileInfo(Operation<FileInfoModel> operation)
        {
            var fs = this.GetProvider();

            return OperationResult<FastDFSFileInfoModel>.OK(await fs.GetFileInfo(operation.Parameter));
        }

        public async ValueTask<OperationResult<StorageNodeModel>> GetStorageNodeAsync(Operation<FileGroupModel> operation)
        {
            var fs = this.GetProvider();

            return OperationResult<StorageNodeModel>.OK(await fs.GetStorageNodeAsync(operation.Parameter));
        }

        public OperationResult<string> GetToken(Operation<FilePlaceModel> operation, DateTime? dateTime = null)
        {
            var fs = this.GetProvider();

            return OperationResult<string>.OK(fs.GetToken(operation.Parameter, dateTime));
        }

        public async ValueTask<IReadOnlyCollection<GroupInfoModel>> ListAllGroupInfosAsync(Operation<string> operation)
        {
            var fs = this.GetProvider();

            return await fs.ListAllGroupInfosAsync(operation.Parameter);
        }

        public async ValueTask<OperationResult<GroupInfoModel>> ListOneGroupInfoAsync(Operation<FileGroupModel> operation)
        {
            var fs = this.GetProvider();

            return OperationResult<GroupInfoModel>.OK(await fs.ListOneGroupInfoAsync(operation.Parameter));
        }

        public async ValueTask<IReadOnlyCollection<StorageInfoModel>> ListStorageInfosAsync(Operation<FileGroupModel> operation)
        {
            var fs = this.GetProvider();

            return await fs.ListStorageInfosAsync(operation.Parameter);
        }

        public async ValueTask<OperationResult<StorageNodeModel>> QueryStorageNodeForFileAsync(Operation<FileInfoModel> operation)
        {
            var fs = this.GetProvider();

            return OperationResult<StorageNodeModel>.OK(await fs.QueryStorageNodeForFileAsync(operation.Parameter));
        }

        public async ValueTask<IReadOnlyCollection<StorageNodeModel>> QueryStorageNodesForFileAsync(Operation<FileInfoModel> operation)
        {
            var fs = this.GetProvider();

            return await fs.QueryStorageNodesForFileAsync(operation.Parameter);
        }

        public async ValueTask<OperationResult<bool>> RemoveFileAsync(Operation<FileInfoModel> operation)
        {
            var fs = this.GetProvider();

            return OperationResult<bool>.OK(await fs.RemoveFileAsync(operation.Parameter));
        }

        public async ValueTask<OperationResult<string>> UploadFileAsync(Operation<UploadModel> operation, CancellationToken cancellationToken = default)
        {
            var fs = this.GetProvider();

            var model = operation.Parameter;

            string fileId = string.Empty;
            await using var transaction = await _repo.Context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var __buffer = new Memory<byte>(new byte[model.FileStream.Length]);
                await model.FileStream.ReadAsync(__buffer, cancellationToken);

                model.FileStream.Position = 0;

                fileId = await fs.UploadFileAsync(operation.Parameter);

                var groupModel = new FileGroupModel { GroupName = model.GroupName, ClusterName = model.ClusterName };

                var groupInfo = await fs.ListOneGroupInfoAsync(groupModel);
                var storageNode = await fs.GetStorageNodeAsync(groupModel);

                _xxh.Update(__buffer.ToArray(), 0, __buffer.Length);

                var hash = Convert.ToBase64String(_xxh.DigestBytes());

                var attachment = new Attachment
                {
                    Id = fileId,
                    Url = $"http://{storageNode.ConnectionAddress.IPAddress}:{groupInfo.StorageHttpPort}/{model.GroupName}/{fileId}",
                    GroupName = model.GroupName,
                    Name = model.FileName,
                    Hash = hash
                };

                await _repo.InsertAsync(attachment, cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return OperationResult<string>.OK(fileId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await fs.RemoveFileAsync(new FileInfoModel { FileId = fileId, GroupName = model.GroupName, ClusterName = model.ClusterName });

                return OperationResult<string>.Error(ex);
            }
            finally
            {
                _xxh.Reset();
            }
        }

        private IFileStorageProvider GetProvider()
        {
            return _provider.GetRequiredService<IFileStorageProvider>();
        }
    }
}