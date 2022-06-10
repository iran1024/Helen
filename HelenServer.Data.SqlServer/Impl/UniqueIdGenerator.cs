using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace HelenServer.Data.SqlServer
{
    public sealed class UniqueIdGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => true;

        public override string Next(EntityEntry entry)
        {
            return Helen.NewGuid;
        }
    }
}