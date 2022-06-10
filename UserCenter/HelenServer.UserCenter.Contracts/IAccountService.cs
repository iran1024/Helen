using HelenServer.Core;
using HelenServer.UserCenter.Shared;

namespace HelenServer.UserCenter.Contracts
{
    public interface IAccountService : IUserService
    {
        Task<OperationResult> SendVerifyCodeAsync(RegisterModel model, CancellationToken cancellationToken);

        Task<OperationResult> RegistAsync(RegisterModel model, CancellationToken cancellationToken);

        Task<OperationResult> UpdatePasswordAsync(RegisterModel model, CancellationToken cancellationToken);

        Task<OperationResult> ExistsAccountAsync(string account, CancellationToken cancellationToken);
    }
}