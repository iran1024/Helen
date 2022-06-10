namespace HelenServer.BugEngine.Contracts;
public class AttachmenModel
{
    public string GroupName { get; set; } = string.Empty;

    public string ClusterName { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string FileExt { get; set; } = string.Empty;

    public Stream FileStream { get; set; } = null!;
}