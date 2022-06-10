namespace HelenServer.BugEngine.Contracts
{
    public interface IDalMigrateService
    {
        Task<bool> MigrateAsync(IEnumerable<ZentaoBugModel> bugs, CancellationToken cancellationToken = default);
    }
}