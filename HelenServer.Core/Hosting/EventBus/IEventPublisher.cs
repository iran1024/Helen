namespace HelenServer.Core;

public interface IEventPublisher
{
    Task PublishAsync(object data, string? name = null, ConsumeMode consumeMode = ConsumeMode.OnceConsume, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
}
