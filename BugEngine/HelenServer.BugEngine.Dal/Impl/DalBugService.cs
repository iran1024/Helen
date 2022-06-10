using AutoMapper;
using HelenServer.BugEngine.Contracts;
using HelenServer.Core;
using HelenServer.Data.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace HelenServer.BugEngine.Dal
{
    [ServiceProvider(typeof(IDalBugService), SqlServerConstants.ProviderName)]
    public class DalBugService : IDalBugService
    {
        private IReadOnlyRepo<BugEngineDbContext> _readRepo;
        private IRepo<BugEngineDbContext> _writeRepo;

        private readonly IMapper _mapper;

        public DalBugService(IReadWriteSplittingDbContextFactory<BugEngineDbContext> factory, IMapper mapper)
        {
            _readRepo = new ReadOnlyRepo<BugEngineDbContext>(factory.Create(ReadWritePolicy.ReadOnly));
            _writeRepo = new Repo<BugEngineDbContext>(factory.Create(ReadWritePolicy.ReadWrite));

            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<BugModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var bugs = await _readRepo.GetAll<Bug>().ToListAsync(cancellationToken);

            var pm = bugs.Select(p => _mapper.Map<Bug, BugModel>(p)).ToList();

            return pm.AsReadOnly();
        }

        public async Task<OperationResult<int>> InsertAsync(Operation<BugModel> operation, CancellationToken cancellationToken = default)
        {
            return OperationResult<int>.OK(await _writeRepo.InsertAsync(_mapper.Map<Bug>(operation.Parameter), cancellationToken));
        }

        public async Task<OperationResult<int>> InsertRangeAsync(Operation<IEnumerable<BugModel>> operation, CancellationToken cancellationToken = default)
        {
            return OperationResult<int>.OK(await _writeRepo.InsertRangeAsync(_mapper.Map<IEnumerable<Bug>>(operation.Parameter), cancellationToken));
        }

        public async Task<OperationResult<BugModel>> FindAsync(Operation<int> operation, CancellationToken cancellationToken = default)
        {
            var entity = await _readRepo.FindAsync<Bug>(n => n.Id == operation.Parameter, cancellationToken: cancellationToken);

            if (entity is null)
            {
                return OperationResult<BugModel>.Failed("无此实体");
            }

            return OperationResult<BugModel>.OK(_mapper.Map<Bug, BugModel>(entity));
        }

        public async Task<OperationResult<int>> DeleteAsync(Operation<int> operation, CancellationToken cancellationToken = default)
        {
            var entity = await _readRepo.FindAsync<Bug>(n => n.Id == operation.Parameter, cancellationToken: cancellationToken);

            if (entity is null)
            {
                return OperationResult<int>.Failed("无此实体");
            }

            return OperationResult<int>.OK(await _writeRepo.RemoveAsync(entity, cancellationToken));
        }

        public async Task<OperationResult<int>> UpdateAsync(Operation<BugModel> operation, CancellationToken cancellationToken = default)
        {
            var entity = await _readRepo.FindAsync<Bug>(n => n.Id == operation.Parameter.Id, cancellationToken: cancellationToken);

            if (entity is null)
            {
                return OperationResult<int>.Failed("无此实体");
            }

            _writeRepo.Context.Attach(entity);

            entity = _mapper.Map<BugModel, Bug>(operation.Parameter);

            return OperationResult<int>.OK(await _writeRepo.UpdateAsync(entity, cancellationToken));
        }
    }
}