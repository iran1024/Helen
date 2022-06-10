using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HelenServer.Data.SqlServer
{
    public class CompositStringToInt32ValueComparer : ValueComparer<ICollection<int>>
    {
        public CompositStringToInt32ValueComparer()
            : base(
                  (i, j) => i!.SequenceEqual(j!),
                  i => i.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                  n => n.ToList())
        {

        }
    }
}