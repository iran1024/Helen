using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HelenServer.Data.SqlServer
{
    public class CompositStringToStringValueConverter : ValueConverter<string[], string>
    {
        public CompositStringToStringValueConverter()
            : base(model => string.Join('#', model),
                  provider => provider.Split('#', StringSplitOptions.RemoveEmptyEntries))
        {

        }
    }
}