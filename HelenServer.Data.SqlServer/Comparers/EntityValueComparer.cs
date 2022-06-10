using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HelenServer.Data.SqlServer
{
    public class EntityValueComparer<TEntity> : ValueComparer<TEntity>
    {
        public EntityValueComparer()
            : base(
                  (i, j) => i!.Equals(j!),
                  i => i!.GetHashCode(),
                  n => n)
        {

        }
    }
}