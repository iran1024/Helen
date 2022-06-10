using Microsoft.EntityFrameworkCore;

namespace HelenServer.Data.SqlServer
{
    public interface IRepo<TContext> : IReader, IWriter
        where TContext : DbContext
    {
        TContext Context { get; }

        bool CanWrite { get; }

        IReadOnlyRepo<TContext> AsReadOnly();
    }
}