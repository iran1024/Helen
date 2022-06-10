using Microsoft.EntityFrameworkCore;

namespace HelenServer.Data.SqlServer
{
    public interface IReadOnlyRepo<TContext> : IReader
        where TContext : DbContext
    {
        TContext Context { get; }

        bool CanWrite { get; }
    }
}