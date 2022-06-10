using HelenServer.Core;

namespace HelenServer.BugEngine.Contracts
{
    public interface IProductService
    {
        Task<IReadOnlyCollection<ProductModel>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<OperationResult> InsertAsync(CancellationToken cancellationToken = default);

        Task<OperationResult> DeleteAsync(Operation<string> operation, CancellationToken cancellationToken = default);

        Task<OperationResult> UpdateAsync(Operation<ProductModel> operation, CancellationToken cancellationToken = default);
    }
}