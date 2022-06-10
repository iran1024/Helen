using Microsoft.EntityFrameworkCore;

namespace HelenServer.Data.SqlServer
{
    internal class InternalRepo<TContext> : RepoBase
        where TContext : DbContext
    {
        public TContext Context { get; }

        public InternalRepo(TContext context)
        {
            Context = context;
        }

        protected IQueryable<TEntity> GetDbSet<TEntity>(bool noTracking)
            where TEntity : class, new()
        {
            if (noTracking)
                return Context.Set<TEntity>().AsNoTracking();
            else
                return Context.Set<TEntity>();
        }

        #region Read        
        public override async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
        {
            var dbSet = GetDbSet<TEntity>(true);

            return await dbSet.AnyAsync(whereExpression, cancellationToken);
        }

        public override async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default)
        {
            var dbSet = GetDbSet<TEntity>(true);

            return await dbSet.CountAsync(whereExpression, cancellationToken);
        }

        public override async Task<TEntity?> FindAsync<TEntity>(
            Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null,
            bool noTracking = true,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var query = GetDbSet<TEntity>(noTracking);

            if (navigationPropertyPath != null)
                return await query.Include(navigationPropertyPath).FirstOrDefaultAsync(cancellationToken);
            else
                return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public override async Task<TEntity?> FindAsync<TEntity>(
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, dynamic>>? navigationPropertyPath = null,
            Expression<Func<TEntity, object>>? orderByExpression = null,
            bool ascending = false,
            bool noTracking = true,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            TEntity? result;

            var query = GetDbSet<TEntity>(noTracking).Where(whereExpression);

            if (navigationPropertyPath != null)
                query = query.Include(navigationPropertyPath);

            if (orderByExpression == null)
                result = await query.FirstOrDefaultAsync(cancellationToken);
            else
                result = ascending
                          ? await query.OrderBy(orderByExpression).FirstOrDefaultAsync(cancellationToken)
                          : await query.OrderByDescending(orderByExpression).FirstOrDefaultAsync(cancellationToken)
                          ;

            return result;
        }

        public override IQueryable<TEntity> GetAll<TEntity>(bool noTracking = true)
        {
            return GetDbSet<TEntity>(noTracking);
        }

        public override async Task<IPageModel<TEntity>> PageAsync<TEntity>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool ascending = false, CancellationToken cancellationToken = default)
        {
            var dbSet = GetDbSet<TEntity>(false);

            var total = await dbSet.CountAsync(whereExpression, cancellationToken);

            if (total == 0)
                return new PageModel<TEntity>() { PageSize = pageSize };

            if (pageIndex <= 0)
                pageIndex = 1;

            if (pageSize <= 0)
                pageSize = 10;

            var query = dbSet.Where(whereExpression);
            query = ascending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);

            var data = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArrayAsync(cancellationToken);

            return new PageModel<TEntity>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = total,
                Data = data
            };
        }

        public override IQueryable<TEntity> Where<TEntity>(Expression<Func<TEntity, bool>> expression, bool noTracking = true)
        {
            return GetDbSet<TEntity>(noTracking).Where(expression);
        }
        #endregion

        #region Write        
        public override async Task<int> RemoveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            Context.Set<TEntity>().Remove(entity);

            return await Context.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> RemoveRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            Context.Set<TEntity>().RemoveRange(entities);

            return await Context.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            await Context.Set<TEntity>().AddAsync(entity, cancellationToken);

            return await Context.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> InsertRangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

            return await Context.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached)
                throw new ArgumentException($"实体没有被跟踪，需要指定更新的列");

            if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
                throw new ArgumentException($"{nameof(entity)}，实体状态为{nameof(entry.State)}");

            return await Context.SaveChangesAsync(cancellationToken);
        }

        public override async Task<int> UpdateAsync<TEntity>(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default)
        {
            if (updatingExpressions.IsNullOrEmpty())
                await UpdateAsync(entity, cancellationToken);

            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Added || entry.State == EntityState.Deleted)
                throw new ArgumentException($"{nameof(entity)}，实体状态为{nameof(entry.State)}");

            if (entry.State == EntityState.Unchanged)
                return await Task.FromResult(0);

            if (entry.State == EntityState.Modified)
            {
                var propNames = updatingExpressions.Select(x => x.GetMemberName()).ToArray();
                entry.Properties.ForEach(propEntry =>
                {
                    if (!propNames.Contains(propEntry.Metadata.Name))
                        propEntry.IsModified = false;
                });
            }

            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Unchanged;
                updatingExpressions.ForEach(expression =>
                {
                    entry.Property(expression).IsModified = true;
                });
            }

            return await Context.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}