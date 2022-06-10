using HelenServer.Core;

namespace HelenServer.BugEngine.Contracts
{
    public interface IBugService
    {
        Task<IReadOnlyCollection<BugModel>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<OperationResult> InsertAsync(Operation<BugModel> operation, CancellationToken cancellationToken = default);

        Task<OperationResult> DeleteAsync(Operation<int> operation, CancellationToken cancellationToken = default);

        Task<OperationResult> UpdateAsync(Operation<BugModel> operation, CancellationToken cancellationToken = default);
    }
}