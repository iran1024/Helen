using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HelenServer.Data.SqlServer
{
    public class EntityValueConverter<TContext, TEntity> : ValueConverter<TEntity, int>
        where TContext : DbContext
        where TEntity : class
    {
        private static readonly TContext _context;

        static EntityValueConverter()
        {
            using var scope = Helen.Services.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<TContext>();
        }

        public EntityValueConverter()
            : base(
                  model => ParseId(model),
                  provider => ConvertFromId(provider))
        {

        }

        private static int ParseId(TEntity model)
        {
            var type = typeof(TEntity);

            var prop = type.GetProperty("Id") ?? throw new InvalidOperationException("模型必须具有属性Id");

            return int.Parse(prop.GetValue(model)!.ToString()!);
        }

        private static TEntity ConvertFromId(int provider)
            => _context.Set<TEntity>().Find(provider)!;
    }
}