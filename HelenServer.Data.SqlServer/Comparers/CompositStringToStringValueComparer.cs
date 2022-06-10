using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HelenServer.Data.SqlServer
{
    public class CompositStringToStringValueComparer : ValueComparer<string[]>
    {
        public CompositStringToStringValueComparer()
            : base(
                  (i, j) => i!.SequenceEqual(j!),
                  i => i.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                  n => n.ToArray())
        {

        }
    }
}