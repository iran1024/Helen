using HelenServer.BugEngine.Contracts;
using HelenServer.Core;

namespace HelenServer.BugEngine.Services
{
    [Injection(typeof(IMigrateService))]
    internal class MigrateService : IMigrateService
    {
        private readonly IDalMigrateService _service;

        public MigrateService(IDalMigrateService service)
        {
            _service = service;
        }

        public async Task<bool> MigrateAsync(string xml, CancellationToken cancellationToken = default)
        {
            var bugs = new ZentaoBugAnalyzer(xml).Analyze();

            if (bugs is null)
            {
                return false;
            }

            return await _service.MigrateAsync(bugs, cancellationToken);
        }
    }
}