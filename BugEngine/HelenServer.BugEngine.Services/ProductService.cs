using HelenServer.BugEngine.Contracts;
using HelenServer.Core;

namespace HelenServer.BugEngine.Services
{
    [Injection(typeof(IProductService))]
    public class ProductService : IProductService
    {
        private readonly IDalProductService _service;

        public ProductService(IDalProductService service)
        {
            _service = service;
        }

        public Task<OperationResult> DeleteAsync(Operation<string> operation, CancellationToken cancellationToken = default)
        {
            return _service.DeleteAsync(operation, cancellationToken);
        }

        public Task<IReadOnlyCollection<ProductModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _service.GetAllAsync(cancellationToken);
        }

        public Task<OperationResult> InsertAsync(CancellationToken cancellationToken = default)
        {
            return _service.InsertAsync(cancellationToken);
        }

        public Task<OperationResult> UpdateAsync(Operation<ProductModel> operation, CancellationToken cancellationToken = default)
        {
            return _service.UpdateAsync(operation, cancellationToken);
        }
    }
}
