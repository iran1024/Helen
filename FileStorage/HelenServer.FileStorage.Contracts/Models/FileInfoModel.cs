namespace HelenServer.FileStorage.Contracts
{
    public class FileInfoModel
    {
        public string FileId { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public string ClusterName { get; set; } = string.Empty;
    }
}