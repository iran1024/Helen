using Microsoft.EntityFrameworkCore;

namespace HelenServer.Data.SqlServer
{
    public class ReadOnlyRepo<TContext> : IReadOnlyRepo<TContext>
        where TContext : DbContext
    {
        public TContext Context { get; }

        public bool CanWrite { get; private set; }

        private readonly InternalRepo<TContext> _innerRepo;

        public ReadOnlyRepo(TContext context)
        {
            Context = context;

            CanWrite = false;

            _innerRepo = new InternalRepo<TContext>(context);
        }

        public IQueryable<TEntity> GetAll<TEntity>(bool noTracking = true) where TEntity : class, new()
        {
            return _innerRepo.GetAll<TEntity>();
        }

        public async Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, bool noTracking = true, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await _innerRepo.FindAsync(navigationPropertyPath, noTracking, cancellationToken);
        }

        public async Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null, Expression<Func<TEntity, object>>? orderByExpression = null, bool ascending = false, bool noTracking = true, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await _innerRepo.FindAsync(whereExpression, navigationPropertyPath, orderByExpression, false, noTracking, cancellationToken);
        }

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await _innerRepo.AnyAsync(whereExpression, cancellationToken);
        }

        public async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await _innerRepo.CountAsync(whereExpression, cancellationToken);
        }

        public IQueryable<TEntity> Where<TEntity>(Expression<Func<TEntity, bool>> expression, bool noTracking = true) where TEntity : class, new()
        {
            return _innerRepo.Where(expression, true);
        }

        public async Task<IPageModel<TEntity>> PageAsync<TEntity>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            return await _innerRepo.PageAsync(pageIndex, pageSize, whereExpression, orderByExpression, ascending, cancellationToken);
        }
    }
}