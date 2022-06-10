using HelenServer.Core;
using HelenServer.UserCenter.Shared;

namespace HelenServer.UserCenter.Contracts
{
    public interface IUserService
    {
        Task<OperationResult<UserModel>> GetAsync(OperationUser operation, CancellationToken cancellationToken);

        Task<IReadOnlyCollection<UserModel>> GetListAsync(Operation<UserSearchModel> operation, CancellationToken cancellationToken);

        Task<OperationResult> DeleteAsync(Operation<string> operation, CancellationToken cancellationToken);

        Task<OperationResult> UpdateAsync(Operation<UserModel> operation, CancellationToken cancellationToken);

        Task<OperationResult> UpdateStatusAsync(Operation<UserStatusModel> operation, CancellationToken cancellationToken);

        Task<bool> ExistAsync(RegisterModel registerModel, CancellationToken cancellationToken);
    }
}
