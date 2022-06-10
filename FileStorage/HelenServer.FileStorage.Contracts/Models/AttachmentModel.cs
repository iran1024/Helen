namespace HelenServer.FileStorage.Contracts
{
    public class AttachmentModel
    {
        public string Id { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Hash { get; set; } = string.Empty;
    }
}