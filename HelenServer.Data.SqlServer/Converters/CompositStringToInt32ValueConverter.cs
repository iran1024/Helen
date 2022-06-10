using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HelenServer.Data.SqlServer
{
    public class CompositStringToInt32ValueConverter : ValueConverter<ICollection<int>, string>
    {
        public CompositStringToInt32ValueConverter()
            : base(
                model => string.Join('#', model),
                provider => Array.ConvertAll(provider.Split('#', StringSplitOptions.RemoveEmptyEntries), s => StringToInt32(s)))
        {

        }

        private static int StringToInt32(string value)
        {
            if (int.TryParse(value, out var num))
            {
                return num;
            }

            return -1;
        }
    }
}