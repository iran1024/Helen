using AutoMapper;
using HelenServer.BugEngine.Contracts;
using HelenServer.Core;
using HelenServer.Data.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace HelenServer.BugEngine.Dal
{
    [ServiceProvider(typeof(IDalProductService), SqlServerConstants.ProviderName)]
    public class DalProductService : IDalProductService
    {
        private IReadOnlyRepo<BugEngineDbContext> _readRepo;
        private IRepo<BugEngineDbContext> _writeRepo;

        private readonly IMapper _mapper;

        public DalProductService(IReadWriteSplittingDbContextFactory<BugEngineDbContext> factory, IMapper mapper)
        {
            _readRepo = new ReadOnlyRepo<BugEngineDbContext>(factory.Create(ReadWritePolicy.ReadOnly));
            _writeRepo = new Repo<BugEngineDbContext>(factory.Create(ReadWritePolicy.ReadWrite));

            _mapper = mapper;
        }

        public async Task<OperationResult> DeleteAsync(Operation<string> operation, CancellationToken cancellationToken = default)
        {
            var entity = await _readRepo.FindAsync<Product>(n => n.Id.ToString() == operation.Parameter, cancellationToken: cancellationToken);

            if (entity is null)
            {
                return OperationResult.Failed("无此实体");
            }

            await _writeRepo.RemoveAsync(entity, cancellationToken);

            return OperationResult.OK();
        }

        public async Task<IReadOnlyCollection<ProductModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var products = await _readRepo.GetAll<Product>().ToListAsync(cancellationToken);

            var pm = products.Select(p => _mapper.Map<Product, ProductModel>(p)).ToList();

            return pm.AsReadOnly();
        }

        public async Task<OperationResult> InsertAsync(CancellationToken cancellationToken = default)
        {
            var entity = new Product() { Id = 5, Name = "PowerServer" };

            await _writeRepo.InsertAsync(entity, cancellationToken);

            return OperationResult.OK();
        }

        public async Task<OperationResult> UpdateAsync(Operation<ProductModel> operation, CancellationToken cancellationToken = default)
        {
            var entity = await _writeRepo.FindAsync<Product>(n => n.Id.ToString() == "5", cancellationToken: cancellationToken);

            var entry = _writeRepo.Context.Attach<Product>(entity);

            entity.Name = "HHHH";

            await _writeRepo.UpdateAsync(entity, cancellationToken);

            return OperationResult.OK();
        }
    }
}