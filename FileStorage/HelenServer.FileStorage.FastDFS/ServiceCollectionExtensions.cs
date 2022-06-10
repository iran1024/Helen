using FastDFSCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDistributedFileStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFastDFSCore(cfg =>
            {
                var config = configuration.GetSection("FastDFS");
                var name = config.GetValue<string>("name");
                var trackers = config.GetSection("trackers").GetChildren().Select(n => new Tracker { IPAddress = n.GetSection("ip").Value, Port = int.Parse(n.GetSection("port").Value) }).ToList();

                cfg.ClusterConfigurations.Add(new ClusterConfiguration
                {
                    Name = name,
                    Trackers = trackers,
                });
            }).AddFastDFSDotNetty();

            return services;
        }
    }
}