namespace HelenServer.Data.SqlServer
{
    public class SqlServerExtensionsOptionsBuilder
    {
        public virtual SqlServerExtensionsOptions Options { get; private set; }

        public SqlServerExtensionsOptionsBuilder()
        {
            Options = new();
        }

        public SqlServerExtensionsOptionsBuilder(SqlServerExtensionsOptions options)
        {
            Options = options;
        }
    }
}
