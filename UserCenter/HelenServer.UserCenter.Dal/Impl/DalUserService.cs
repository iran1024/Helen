using HelenServer.Core;
using HelenServer.Data.SqlServer;
using HelenServer.UserCenter.Contracts;
using HelenServer.UserCenter.Shared;

namespace HelenServer.UserCenter.Dal
{
    [ServiceProvider(typeof(IDalUserService), SqlServerConstants.ProviderName)]
    public class DalUserService : IDalUserService
    {
        private readonly Repo<UserCenterDbContext> _repo;

        public DalUserService(UserCenterDbContext context)
        {
            _repo = new Repo<UserCenterDbContext>(context);
        }

        public async Task<int> AddAsync(UserModel userModel, CancellationToken cancellationToken)
        {
            var entity = userModel.ToEntity();

            var r = await _repo.InsertAsync(entity, cancellationToken);

            return r;
        }

        public Task<bool> DeleteAsync(Operation<string> operation, CancellationToken cancellationToken)
        {
            return default;
        }

        public Task<bool> ExistsAsync(string username, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistUserAsync(RegisterModel registerModel, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetAsync(Operation<string> operation, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<UserModel>> GetListAsync(Operation<UserSearchModel> operation, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Operation<UserModel> operation, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePasswordAsync(Operation<UserPasswordModel> operation, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStatusAsync(Operation<UserStatusModel> operatioa, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}