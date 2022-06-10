
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using HelenServer.BugEngine.Contracts;
using HelenServer.BugEngine.Dal;
using HelenServer.Core;
using Microsoft.EntityFrameworkCore;

namespace HelenServer.ConsoleTestApp
{
    class Program
    {
        public static BugEngineDbContext Context { get; private set; } = null!;
        public static IDalBugService BugService { get; private set; } = null!;
        public static IDalProductService ProductService { get; private set; } = null!;

        public static OperationUser User { get; private set; } = new OperationUser("#001", "Ray", new List<string>() { "admin", "tester" }, true);
        static async Task Main()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.AddExpressionMapping().AddProfile<BugProfile>();
            var mapper = new Mapper(new MapperConfiguration(n => n.AddExpressionMapping().AddProfile<BugProfile>()));

            Context = new BugEngineDbContext(new DbContextOptionsBuilder<BugEngineDbContext>().UseSqlServer("Data Source=172.25.13.16;Initial Catalog=helen_master;User ID=sa;Password=appeon;Trust Server Certificate=True").Options);

            BugService = new DalBugService(Context);
            ProductService = new DalProductService(Context);

            var zentaoBugs = new ZentaoBugAnalyzer(File.ReadAllText(@"C:\Users\appeon\Desktop\SnapDevelop 2022-所有Bug.xml")).Analyze()!;

            var bugModels = new List<BugModel>();

            foreach (var bug in zentaoBugs.Bugs)
            {
                bugModels.Add(mapper.Map<BugModel>(bug));
            }

            var operation = new Operation<IEnumerable<BugModel>>(User)
            {
                Parameter = bugModels
            };

            var result = await BugService.InsertRangeAsync(operation, CancellationToken.None);
        }

        private static string ConvertBugStatus(string status)
        {
            return status switch
            {
                "激活" => "Active",
                "已解决" => "Resolved",
                "已关闭" => "Closed",
                _ => throw new NotSupportedException()
            };
        }
    }

    #region MyRegion
    /* 
     *  var bugModel = new BugModel()
                {
                    Id = bug.Id,
                    Title = bug.Title,
                    Product = 1,
                    Module = 1,
                    Demand = bug.Demand,
                    Keywords = bug.Keywords,
                    Severity = bug.Severity,
                    Priority = bug.Priority,
                    Type = 1,
                    Os = 1,
                    Browser = 1,
                    ExpirationTime = bug.ExpirationTime,
                    Reproduce = bug.Reproduce,
                    Status = (int)ConvertBugStatus(bug.Status).ToEnum<BugStatus>(),
                    ActivedCount = bug.ActivedCount,
                    LastActivedTime = new DateTime(1888, 12, 25),
                    IsConfirmed = bug.IsConfirmed == "是",
                    Cclist = bug.CCList,
                    Creator = 1,// todo
                    CreatedTime = bug.CreatedTime,
                    AffectVersions = bug.AffectVersions,
                    Assignment = 1, // todo
                    AssignmentTime = bug.AssignmentTime,
                    ResolvedBy = 1,
                    Resolution = 1,
                    ResolvedTime = bug.ResolvedTime,
                    ResolvedVersion = 1, // todo
                    ClosedBy = 1,
                    ClosedTime = bug.ClosedTime,
                    DuplicateId = bug.DuplicateId,
                    RelatedBugs = bug.RelatedBugs,
                    RelatedCases = bug.RelatedCases,
                    LastEditBy = 1, // todo
                    LastEditTime = bug.LastEditTime,
                    Attachments = string.Empty,
                    OperationLogs = null
                };
     * 
     */
    #endregion
}