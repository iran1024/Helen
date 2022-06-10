namespace HelenServer.Core;

public interface IServiceAnalyzer
{
    bool TryAnalyze(IServiceCollection services, Type type, InjectionAttribute attribute);
}
