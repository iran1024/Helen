using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Transactions;

namespace HelenServer.Data.SqlServer
{
    public class ReadWriteSplittingIntercepter : DbCommandInterceptor
    {
        private readonly string _master;
        private readonly string[] _slave;

        public ReadWriteSplittingIntercepter(IConfiguration cfg)
        {
            _master = cfg.GetConnectionString("master");
            _slave = cfg.GetConnectionString("slave").Split(',').Select(n => n.Trim()).ToArray();
        }

        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {

            return base.NonQueryExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            ConnectionAdapter(command, eventData);

            return base.ReaderExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            ConnectionAdapter(command, eventData);

            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            return base.ScalarExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

        private void ConnectionAdapter(DbCommand command, CommandEventData eventData)
        {
            switch (eventData.CommandSource)
            {
                case CommandSource.Unknown:
                case CommandSource.SaveChanges:
                case CommandSource.BulkUpdate:
                case CommandSource.Migrations:
                    {
                        SwitchToMaster(command);
                    }
                    break;
                case CommandSource.LinqQuery:
                case CommandSource.FromSqlQuery:
                    {
                        SwitchToSlave(command);
                    }
                    break;
                case CommandSource.ExecuteSqlRaw:
                    {
                        if (command.CommandText.ToLower().StartsWith("select", StringComparison.OrdinalIgnoreCase))
                        {
                            SwitchToSlave(command);
                        }
                    }
                    break;
            }
        }

        private void SwitchToMaster(DbCommand command)
        {
            if (!DetectSuitableTransaction(command))
            {
                throw new TransactionException("EFCore内置事务已开启，无法切换主从数据库，请使用<读写分离上下文工厂>构建DbContext");
            }

            if (command.Connection is not null)
            {
                command.Connection.Close();
                command.Connection.ConnectionString = _master;
                command.Connection.Open();
            }
        }

        private void SwitchToSlave(DbCommand command)
        {
            if (!DetectSuitableTransaction(command))
            {
                throw new TransactionException("EFCore内置事务已开启，无法切换主从数据库，请使用<读写分离上下文工厂>构建DbContext");
            }

            if (DetectSuitableTransaction(command) && command.Connection is not null)
            {
                command.Connection.Close();
                command.Connection.ConnectionString = GetSlaveConnectionString();
                command.Connection.Open();
            }
        }

        private static bool DetectSuitableTransaction(DbCommand command)
        {
            bool isDistributedTran = Transaction.Current != null &&
                    Transaction.Current.TransactionInformation.Status != TransactionStatus.Committed;

            bool isDbTran = command.Transaction is not null;

            return !isDistributedTran && !isDbTran;
        }

        private string GetSlaveConnectionString()
        {
            return _slave[0];
        }
    }
}