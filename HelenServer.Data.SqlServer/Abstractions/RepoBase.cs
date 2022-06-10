namespace HelenServer.Data.SqlServer
{
    public abstract class RepoBase
    {
        #region Read
        public abstract IQueryable<TEntity> GetAll<TEntity>(bool noTracking = true)
            where TEntity : class, new();

        public abstract Task<TEntity?> FindAsync<TEntity>(
            Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null,
            bool noTracking = true,
            CancellationToken cancellationToken = default)
            where TEntity : class, new();

        public abstract Task<TEntity?> FindAsync<TEntity>(
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null,
            Expression<Func<TEntity, object>>? orderByExpression = null,
            bool ascending = false,
            bool noTracking = true,
            CancellationToken cancellationToken = default)
            where TEntity : class, new();

        public abstract Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        public abstract Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        public abstract IQueryable<TEntity> Where<TEntity>(Expression<Func<TEntity, bool>> expression, bool noTracking = true)
            where TEntity : class, new();

        public abstract Task<IPageModel<TEntity>> PageAsync<TEntity>(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> orderByExpression,
            bool ascending = false,
            CancellationToken cancellationToken = default)
            where TEntity : class, new();
        #endregion

        #region Write
        public abstract Task<int> RemoveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        public abstract Task<int> RemoveRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        public abstract Task<int> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        public abstract Task<int> InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        public abstract Task<int> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        public abstract Task<int> UpdateAsync<TEntity>(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default)
            where TEntity : class, new();
        #endregion
    }
}