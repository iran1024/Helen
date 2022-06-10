using Microsoft.EntityFrameworkCore;

namespace HelenServer.Data.SqlServer
{
    public interface IReadWriteSplittingDbContextFactory<TContext>
        where TContext : DbContext
    {
        TContext Create(ReadWritePolicy policy);

        static Func<DbContextOptions<TContext>, TContext> Factory { get; } = null!;
    }
}