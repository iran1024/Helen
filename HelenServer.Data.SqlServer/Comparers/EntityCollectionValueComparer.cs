using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HelenServer.Data.SqlServer
{
    public class EntityCollectionValueComparer<TEntity> : ValueComparer<ICollection<TEntity>>
    {
        public EntityCollectionValueComparer()
            : base(
                  (i, j) => i!.SequenceEqual(j!),
                  i => i.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                  n => n.ToList())
        {

        }
    }
}