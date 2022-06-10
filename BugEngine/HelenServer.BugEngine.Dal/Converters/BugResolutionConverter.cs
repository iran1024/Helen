using HelenServer.BugEngine.Contracts;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HelenServer.BugEngine.Dal
{
    public class BugResolutionConverter : ValueConverter<BugResolution, string>
    {
        public BugResolutionConverter()
            : base(
                  model => model.ToString(),
                  provider => ConvertFromString(provider)
                  )
        {

        }

        private static BugResolution ConvertFromString(string provider)
        {
            if (provider.Contains('#'))
            {
                var did = int.Parse(provider.Split('#')[1]);

                return BugResolution.Duplicated.SetDuplicatedId(did);
            }

            return BugResolution.Parse(provider)!;
        }
    }
}
