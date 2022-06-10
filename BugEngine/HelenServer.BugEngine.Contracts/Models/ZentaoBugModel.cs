namespace HelenServer.BugEngine.Contracts
{
    public class ZentaoBugModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Product { get; set; }

        public string Module { get; set; }

        public int Demand { get; set; }

        public string Keywords { get; set; }

        public int Severity { get; set; }

        public int Priority { get; set; }

        public string Type { get; set; }

        public string Os { get; set; }

        public string Browser { get; set; }

        public DateTime? ExpirationTime { get; set; }

        public string Reproduce { get; set; }

        public string Status { get; set; }

        public int ActivedCount { get; set; }

        public DateTime? LastActivedTime { get; set; }

        public string IsConfirmed { get; set; }

        public string CCList { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedTime { get; set; }

        public string AffectVersions { get; set; }

        public string Assignment { get; set; }

        public DateTime? AssignmentTime { get; set; }

        public string ResolvedBy { get; set; }

        public string Resolution { get; set; }

        public DateTime? ResolvedTime { get; set; }

        public string ResolvedVersion { get; set; }

        public string ClosedBy { get; set; }

        public DateTime? ClosedTime { get; set; }

        public int? DuplicateId { get; set; }

        public string RelatedBugs { get; set; }

        public string RelatedCases { get; set; }

        public string LastEditBy { get; set; }

        public DateTime? LastEditTime { get; set; }

        public string Attachments { get; set; }

        public string HistoryRecord { get; set; }
    }
}