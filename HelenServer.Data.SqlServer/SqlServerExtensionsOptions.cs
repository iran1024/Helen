using Microsoft.EntityFrameworkCore;

namespace HelenServer.Data.SqlServer
{
    public class SqlServerExtensionsOptions
    {
        public DbContext? DbContext { get; set; }
        public IAdvancedDistributedCache? Cache { get; set; }

        public IGrpcClientFactory? GrpcClientFactory { get; set; }

        public IEventPublisher? Publisher { get; set; }

        public IEventConsumer? Consumer { get; set; }
    }
}