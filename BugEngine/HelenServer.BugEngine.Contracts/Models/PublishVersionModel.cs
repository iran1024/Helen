namespace HelenServer.BugEngine.Contracts
{
    public class PublishVersionModel
    {
        public int Id { get; set; }

        public string Version { get; set; } = string.Empty;

        public ProductModel Product { get; set; } = null!;
    }
}