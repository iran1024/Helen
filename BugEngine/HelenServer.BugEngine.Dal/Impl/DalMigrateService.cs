using Dapr.Client;
using HelenServer.BugEngine.Contracts;

namespace HelenServer.BugEngine.Dal
{
    [ServiceProvider(typeof(IDalMigrateService), SqlServerConstants.ProviderName)]
    internal class DalMigrateService : IDalMigrateService
    {
        private readonly IRepo<BugEngineDbContext> _repo;
        private readonly DaprClient _daprClient;

        public DalMigrateService(BugEngineDbContext context, DaprClient daprClient)
        {
            _repo = new Repo<BugEngineDbContext>(context);
            _daprClient = daprClient;
        }

        public async Task<bool> MigrateAsync(IEnumerable<ZentaoBugModel> bugs, CancellationToken cancellationToken = default)
        {
            var result = await _daprClient.CheckHealthAsync(cancellationToken);

            await _daprClient.DeleteStateAsync("hs-statestore", "product", cancellationToken: cancellationToken);
            
            var bugModels = new List<BugModel>();

            var products = bugs.Select(n => n.Product).Distinct().Where(n => n != "").OrderBy(n => n[0]).ToList();
            var modules = bugs.Select(n => n.Module).Distinct().Where(n => n != "").OrderBy(n => n[1]).ToList();
            var types = bugs.Select(n => n.Type).Distinct().Where(n => n != "").ToList();
            var affectVersions = bugs.Select(n => (n.Product, n.AffectVersions)).Distinct().Where(n => n.AffectVersions != "" && !n.AffectVersions.Contains("主干")).OrderBy(n => n.AffectVersions[0]).ToList();
            var resolutions = bugs.Select(n => n.Resolution).Distinct().Where(n => n != "").ToList();
            var attachments = bugs.Select(n => n.Attachments).Distinct().Where(n => n != "").ToList();

            var firstAttachment = attachments[0];

            await this.ParseFile(firstAttachment);

            await using var transaction = await _repo.Context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
//                var d_products = products.Select((n, i) => new Product { Name = n.Split('(')[0] }).ToList();
//                await _repo.Context.Product.AddRangeAsync(d_products, cancellationToken);
//                await _repo.Context.SaveChangesAsync(cancellationToken);

//                var d_modules = modules.Select((n, i) => new Module { Name = n.Split('(')[0][1..] }).Where(n => n.Name != "").ToList();
//                d_modules.Insert(0, new Module { Name = @"/" });
//                var d_types = types.Select((n, i) => new BugType { TypeName = n }).ToList();
//                var d_affectVersions = affectVersions.Select((n, i) => new PublishVersion { Product = d_products.Find(x => x.Name == n.Product.Split('(')[0])!, Version = n.AffectVersions.Split('(')[0] }).Distinct(new PublishVersion()).ToList();
//                await Parallel.ForEachAsync(attachments, cancellationToken, (n, cancellationToken) => this.ParseFile(n));

                //await _repo.Context.Module.AddRangeAsync(d_modules, cancellationToken);
                //await _repo.Context.SaveChangesAsync(cancellationToken);
                //await _repo.Context.BugType.AddRangeAsync(d_types, cancellationToken);
                //await _repo.Context.SaveChangesAsync(cancellationToken);
                //await _repo.Context.PublishVersion.AddRangeAsync(d_affectVersions, cancellationToken);
                //await _repo.Context.SaveChangesAsync(cancellationToken);

                //var d_bugs = bugs.Select(n =>
                //{
                //    var model = new Bug();

                //    model.Id = n.Id;
                //    model.Title = n.Title;
                //    model.Product = d_products.First(x => n.Product.Contains(x.Name));
                //    model.Module = d_modules.First(x => n.Module.Contains(x.Name));
                //    model.Demand = n.Demand;
                //    model.Keywords = n.Keywords.TrimEnd().Split(' ');
                //    model.Severity = n.Severity;
                //    model.Priority = n.Priority;
                //    model.Type = d_types.FirstOrDefault(x => x.TypeName == n.Type) ?? d_types[0];
                //    model.Os = string.IsNullOrEmpty(n.Os) ? null : new Os { Version = n.Os };
                //    model.Browser = string.IsNullOrEmpty(n.Browser) ? null : new Browser { Name = n.Browser };
                //    model.ExpirationTime = n.ExpirationTime;
                //    model.Reproduce = n.Reproduce;
                //    model.Status = ParseStatus(n.Status);
                //    model.ActivedCount = n.ActivedCount;
                //    model.LastActivedTime = n.LastActivedTime;
                //    model.IsConfirmed = n.IsConfirmed == "是";
                //    model.Cclist = new List<int>();
                //    model.Creator = 0; // 等用户中心开发完以及Dapr集成进来
                //    model.CreatedTime = n.CreatedTime;
                //    model.AffectVersions = ParsePublishVersion(n.AffectVersions, d_affectVersions);
                //    model.Assignment = 0;
                //    model.AssignmentTime = n.AssignmentTime;
                //    model.ResolvedBy = string.IsNullOrEmpty(n.ResolvedBy) ? null : 0;
                //    model.Resolution = ParseResolution(n.Resolution, n.DuplicateId);
                //    model.ResolvedTime = n.ResolvedTime;
                //    model.ResolvedVersion = ParseResolvedVersion(n.ResolvedVersion, d_affectVersions);
                //    model.ClosedBy = string.IsNullOrEmpty(n.ClosedBy) ? null : 0;
                //    model.ClosedTime = n.ClosedTime;
                //    model.DuplicateId = n.DuplicateId;
                //    model.RelatedBugs = new List<Bug>();
                //    model.RelatedCases = Array.Empty<int>();
                //    model.LastEditBy = string.IsNullOrEmpty(n.LastEditBy) ? null : 0;
                //    model.LastEditTime = n.LastEditTime;
                //    model.Attachments = Array.Empty<string>();
                //    model.OperationLogs = Array.Empty<OperateLog>();

                //    return model;
                //}).ToList();

                //await _repo.Context.Bug.AddRangeAsync(d_bugs, cancellationToken);
                //await _repo.Context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }

            return true;
        }

        private async ValueTask ParseFile(string attachment)
        {
            var path = attachment.Split(' ')[1][6..^1];
            var filename = attachment.Split('>')[1][..^3];
            try
            {
                var handler = new HttpClientHandler() { UseCookies = false };
                using var dc = new HttpClient(handler);
                
                dc.DefaultRequestHeaders.Add("Authorization", "Basic emVudGFvOkFwcGVvbjEyMyFAIw==");
                dc.DefaultRequestHeaders.Add("Cookie", "device=desktop; theme=default; preBranch=0; productStoryOrder=id_desc; moduleBrowseParam=0; productBrowseParam=0; storyModuleParam=0; storyProductParam=0; storyBranchParam=0; projectStoryOrder=order_desc; projectTaskOrder=deadline_asc; keepLogin=on; za=zengzimo; caseModule=0; lang=zh-cn; qaBugOrder=id_desc; storyModule=0; preProductID=13; ajax_lastNext=on; lastProject=9; bugModule=189; checkedItem=10147; docFilesViewType=card; from=doc; lastProduct=13; zp=cc37a9e8bb3ac07d787b2323cba027aa98294e16; fullscreen=false; selfClose=1; downloading=null; windowHeight=1289; windowWidth=1360; zentaosid=ia78ldq5pasg5r0ktdfkl2l623");
                
                var buffer = await dc.GetByteArrayAsync(path);
                
                var client = DaprClient.CreateInvokeHttpClient("fs");

                var content = new MultipartFormDataContent
                {
                    { new ByteArrayContent(buffer), "upload", filename }
                };

                var result = await client.PostAsync("fs/file", content);
            }
            catch (Exception ex)
            {
                
            }            
        }

        private static BugStatus ParseStatus(string val)
        {
            var temp = val == "激活" ? "Active" : val == "已解决" ? "Resolved" : "Closed";

            return Helen.ToEnum<BugStatus>(temp);
        }

        private static PublishVersion[] ParsePublishVersion(string val, IEnumerable<PublishVersion> set)
        {
            var models = new List<PublishVersion>();
            var versions = val.Split(' ');

            foreach (var version in versions)
            {
                if (version.Contains("主干")) continue;
                var v = version.Split('(')[0];

                models.Add(set.First(n => n.Version == v));
            }

            return models.ToArray();
        }

        private static PublishVersion? ParseResolvedVersion(string val, IEnumerable<PublishVersion> set)
        {
            if (string.IsNullOrEmpty(val) || val.Contains("主干"))
            {
                return null;
            }

            return set.First(x => x.Version == val.Split('(')[0]);
        }

        private static BugResolution? ParseResolution(string val, int? did)
        {
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }

            if (did is null)
            {
                return BugResolution.Parse(val)!;
            }

            return BugResolution.Duplicated.SetDuplicatedId(did.Value);
        }
    }
}