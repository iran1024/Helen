using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace HelenServer.Data.SqlServer
{
    public class DefaultIntercepter : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            if (eventData.CommandSource == CommandSource.SaveChanges)
            {
                Console.WriteLine($"{eventData.Context?.GetType().Name}提交了修改");
                Console.WriteLine();
                Console.WriteLine(command.CommandText);
            }

            if (eventData.CommandSource == CommandSource.FromSqlQuery || eventData.CommandSource == CommandSource.LinqQuery)
            {
                Console.WriteLine($"{eventData.Context?.GetType().Name}查询了数据");
                Console.WriteLine();
                Console.WriteLine(command.CommandText);
            }


            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}