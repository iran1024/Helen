namespace HelenServer.BugEngine.Contracts
{
    public class BugModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ProductModel Product { get; set; } = null!;
        public ModuleModel? Module { get; set; } = null!;
        public int? Demand { get; set; }
        public string[] Keywords { get; set; } = Array.Empty<string>();
        public int Severity { get; set; }
        public int Priority { get; set; }
        public BugTypeModel Type { get; set; } = null!;
        public OsModel? Os { get; set; } = null!;
        public BrowserModel? Browser { get; set; } = null!;
        public DateTime? ExpirationTime { get; set; }
        public string? Reproduce { get; set; }
        public BugStatus Status { get; set; }
        public int ActivedCount { get; set; } = 0;
        public DateTime? LastActivedTime { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public int[] Cclist { get; set; } = Array.Empty<int>();
        public int Creator { get; set; }
        public DateTime CreatedTime { get; set; }
        public PublishVersionModel[] AffectVersions { get; set; } = Array.Empty<PublishVersionModel>();
        public int Assignment { get; set; }
        public DateTime? AssignmentTime { get; set; }
        public int? ResolvedBy { get; set; }
        public BugResolution? Resolution { get; set; }
        public DateTime? ResolvedTime { get; set; }
        public PublishVersionModel? ResolvedVersion { get; set; }
        public int? ClosedBy { get; set; }
        public DateTime? ClosedTime { get; set; }
        public int? DuplicateId { get; set; }
        public BugModel[]? RelatedBugs { get; set; }
        public int[]? RelatedCases { get; set; }
        public int? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }
        public string[]? Attachments { get; set; }
        public OperationLogModel[] OperationLogs { get; set; } = Array.Empty<OperationLogModel>();
    }
}