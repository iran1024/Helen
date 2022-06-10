using HelenServer.BugEngine.Contracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HelenServer.BugEngine.Dal
{
    public class BugResolutionComparer : ValueComparer<BugResolution>
    {
        public BugResolutionComparer()
            : base(
                  (i, j) => i!.Equals(j),
                  i => i.GetHashCode(),
                  n => n)
        {

        }
    }
}