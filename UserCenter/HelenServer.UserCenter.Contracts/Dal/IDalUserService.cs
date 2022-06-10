using HelenServer.Core;
using HelenServer.UserCenter.Shared;

namespace HelenServer.UserCenter.Contracts
{
    public interface IDalUserService
    {
        Task<int> AddAsync(UserModel userModel, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(Operation<string> operation, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(Operation<UserModel> operation, CancellationToken cancellationToken);

        Task<UserModel> GetAsync(Operation<string> operation, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<UserModel>> GetListAsync(Operation<UserSearchModel> operation, CancellationToken cancellationToken);

        Task<bool> ExistUserAsync(RegisterModel registerModel, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string username, CancellationToken cancellationToken);

        Task<bool> UpdatePasswordAsync(Operation<UserPasswordModel> operation, CancellationToken cancellationToken);

        Task<bool> UpdateStatusAsync(Operation<UserStatusModel> operatioa, CancellationToken cancellationToken);
    }
}