namespace HelenServer.BugEngine.Contracts
{
    public class OperationLogModel
    {
        public string Operator { get; set; } = string.Empty;

        public DateTime OperatedTime { get; set; }

        public OperationType OperationType { get; set; }

        public BugResolution? Resolution { get; set; }

        public string Remark { get; set; } = string.Empty;
    }
}