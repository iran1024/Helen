using HelenServer.BugEngine.Contracts;
using HelenServer.Core;

namespace HelenServer.BugEngine.Services
{
    [Injection(typeof(IBugService))]
    public class BugService : IBugService
    {
        private readonly IDalBugService _service;

        public BugService(IDalBugService service)
        {
            _service = service;
        }

        public async Task<OperationResult> DeleteAsync(Operation<int> operation, CancellationToken cancellationToken = default)
        {
            return await _service.DeleteAsync(operation, cancellationToken);
        }

        public async Task<IReadOnlyCollection<BugModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        public async Task<OperationResult> InsertAsync(Operation<BugModel> operation, CancellationToken cancellationToken = default)
        {
            return await _service.InsertAsync(operation, cancellationToken);
        }

        public async Task<OperationResult> UpdateAsync(Operation<BugModel> operation, CancellationToken cancellationToken = default)
        {
            return await _service.UpdateAsync(operation, cancellationToken);
        }
    }
}
