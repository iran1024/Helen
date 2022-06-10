namespace HelenServer.Core;

public interface IServiceFactory
{
    object GetService(IServiceProvider provider);
}
