namespace HelenServer.Data.SqlServer
{
    public interface IWriter
    {
        Task<int> RemoveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        Task<int> RemoveRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        Task<int> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        Task<int> InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        Task<int> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, new();

        Task<int> UpdateAsync<TEntity>(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default)
            where TEntity : class, new();
    }
}