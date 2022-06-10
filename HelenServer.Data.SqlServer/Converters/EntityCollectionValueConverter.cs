using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HelenServer.Data.SqlServer
{
    public class EntityCollectionValueConverter<TContext, TEntity> : ValueConverter<ICollection<TEntity>, string>
        where TContext : DbContext
        where TEntity : class
    {
        private static readonly TContext _context;

        static EntityCollectionValueConverter()
        {
            using var scope = Helen.Services.CreateScope();

            _context = scope.ServiceProvider.GetRequiredService<TContext>();
        }

        public EntityCollectionValueConverter()
            : base(
                  model => CompositId(model),
                  provider => ConvertFromIds(provider))
        {

        }

        private static string CompositId(ICollection<TEntity> models)
        {
            var type = typeof(TEntity);

            var prop = type.GetProperty("Id") ?? throw new InvalidOperationException("模型必须具有属性Id");

            var temp = new List<string>();

            foreach (var model in models)
            {
                temp.Add(prop.GetValue(model)!.ToString()!);
            }

            return string.Join('#', temp);
        }

        private static ICollection<TEntity> ConvertFromIds(string provider)
        {
            var ids = provider.Split('#');

            var models = new List<TEntity>();

            foreach (var id in ids)
            {
                models.Add(_context.Set<TEntity>().Find(id)!);
            }

            return models;
        }
    }
}