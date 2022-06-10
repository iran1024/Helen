namespace HelenServer.BugEngine.Contracts
{
    public interface IMigrateService
    {
        Task<bool> MigrateAsync(string xml, CancellationToken cancellationToken = default);
    }
}