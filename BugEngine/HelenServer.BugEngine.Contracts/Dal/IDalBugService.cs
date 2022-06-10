using HelenServer.Core;

namespace HelenServer.BugEngine.Contracts
{
    public interface IDalBugService
    {
        Task<IReadOnlyCollection<BugModel>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<OperationResult<int>> InsertAsync(Operation<BugModel> operation, CancellationToken cancellationToken = default);

        Task<OperationResult<int>> InsertRangeAsync(Operation<IEnumerable<BugModel>> operation, CancellationToken cancellationToken = default);

        Task<OperationResult<BugModel>> FindAsync(Operation<int> operation, CancellationToken cancellationToken = default);

        Task<OperationResult<int>> DeleteAsync(Operation<int> operation, CancellationToken cancellationToken = default);

        Task<OperationResult<int>> UpdateAsync(Operation<BugModel> operation, CancellationToken cancellationToken = default);
    }
}