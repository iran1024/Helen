namespace HelenServer.Data.SqlServer
{
    public interface IReader
    {
        IQueryable<TEntity> GetAll<TEntity>(bool noTracking = true)
            where TEntity : class, new();

        Task<TEntity?> FindAsync<TEntity>(
            Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null,
            bool noTracking = true,
            CancellationToken cancellationToken = default)
            where TEntity : class, new();

        Task<TEntity?> FindAsync<TEntity>(
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null,
            Expression<Func<TEntity, object>>? orderByExpression = null,
            bool ascending = false,
            bool noTracking = true,
            CancellationToken cancellationToken = default)
            where TEntity : class, new();

        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        IQueryable<TEntity> Where<TEntity>(Expression<Func<TEntity, bool>> expression, bool noTracking = true)
            where TEntity : class, new();

        Task<IPageModel<TEntity>> PageAsync<TEntity>(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> orderByExpression,
            bool ascending = false,
            CancellationToken cancellationToken = default)
            where TEntity : class, new();
    }
}