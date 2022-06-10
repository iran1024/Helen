using Microsoft.EntityFrameworkCore;

namespace HelenServer.Data.SqlServer
{
    public class ReadWriteSplittingDbContextFactory<TContext> : IReadWriteSplittingDbContextFactory<TContext>
        where TContext : DbContext
    {
        private readonly IConfiguration _config;
        private readonly string _master;
        private readonly string[] _slave;
        public static Func<DbContextOptions<TContext>, TContext> Factory { get; }

        static ReadWriteSplittingDbContextFactory()
        {
            var parameter = Expression.Parameter(typeof(DbContextOptions<TContext>), "options");

            var constructor = typeof(TContext).GetConstructor(
                BindingFlags.Public | BindingFlags.Instance,
                null,
                CallingConventions.HasThis,
                new[] { typeof(DbContextOptions<TContext>) }, Array.Empty<ParameterModifier>())
                    ?? throw new Exception("缺少参数为'DbContextOptions<TContext>'类型的构造函数");

            Factory = Expression.Lambda<Func<DbContextOptions<TContext>, TContext>>(
                Expression.New(constructor, parameter), new ParameterExpression[] { parameter }).Compile();
        }

        public ReadWriteSplittingDbContextFactory(IConfiguration config)
        {
            _config = config;
            _master = _config.GetConnectionString("master");
            _slave = _config.GetConnectionString("slave").Split(',').Select(n => n.Trim()).ToArray();
        }

        public TContext Create(ReadWritePolicy policy)
        {
            return policy switch
            {
                ReadWritePolicy.ReadOnly => DynamicCreateDbContext(builder =>
                    builder.EnableDetailedErrors().UseLazyLoadingProxies().UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll).UseSqlServer(GetSlaveConnectionString()).AddInterceptors(new DefaultIntercepter()).Options),
                ReadWritePolicy.ReadWrite => DynamicCreateDbContext(builder =>
                    builder.EnableDetailedErrors().UseLazyLoadingProxies().UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll).UseSqlServer(_master).AddInterceptors(new DefaultIntercepter()).Options),
                _ => throw new NotImplementedException(),
            };
        }

        private static TContext DynamicCreateDbContext(Func<DbContextOptionsBuilder<TContext>, DbContextOptions<TContext>> optionsBuilder)
            => Factory.Invoke(optionsBuilder(new DbContextOptionsBuilder<TContext>()));

        private string GetSlaveConnectionString()
        {
            return _slave[0];
        }
    }
}