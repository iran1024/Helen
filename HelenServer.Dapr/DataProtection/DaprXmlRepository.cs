using Microsoft.AspNetCore.DataProtection.Repositories;
using System.Xml.Linq;

namespace HelenServer.Dapr.DataProtection
{
    public class DaprXmlRepository : IXmlRepository
    {
        private readonly IAdvancedDistributedCache _cache;
        private readonly string _key;

        public DaprXmlRepository(IAdvancedDistributedCache cacheService, string key)
        {
            _cache = cacheService;
            _key = key;
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return GetAllElementsCore().ToList().AsReadOnly();
        }

        private IEnumerable<XElement> GetAllElementsCore()
        {
            var docString = _cache.Get<string>(_key);

            if (!string.IsNullOrWhiteSpace(docString))
            {
                yield return XElement.Parse(docString);
            }
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            _cache.Set(_key, element.ToString(SaveOptions.DisableFormatting));
        }
    }
}