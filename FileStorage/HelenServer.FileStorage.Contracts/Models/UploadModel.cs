namespace HelenServer.FileStorage.Contracts
{
    public class UploadModel
    {
        public string GroupName { get; set; } = string.Empty;

        public string ClusterName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string FileExt { get; set; } = string.Empty;

        public Stream FileStream { get; set; } = null!;
    }
}