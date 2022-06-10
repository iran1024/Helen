using Dapr.Client;

namespace HelenServer.Dapr;

internal class DaprEventPublisher : IEventPublisher
{
    private readonly DaprClient _daprClient;
    private readonly DaprEventBusOptions _options;

    public DaprEventPublisher(DaprClient daprClient)
    {
        _daprClient = daprClient;
        _options = new DaprEventBusOptions();
    }

    public Task PublishAsync(
        object data, string? name = null, ConsumeMode mode = ConsumeMode.OnceConsume,
        Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        name ??= data.GetType().Name;

        var publisher = _options.Components.FirstOrDefault(p => p.Mode == mode);

        if (publisher == null)
        {
            throw new ArgumentNullException(nameof(DaprEventBusOptions.Components));
        }

        if (headers == null)
        {
            return _daprClient.PublishEventAsync(publisher.PubSubName, name, data, cancellationToken);
        }
        else
        {
            return _daprClient.PublishEventAsync(publisher.PubSubName, name, data, headers, cancellationToken);
        }
    }
}
