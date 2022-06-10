namespace HelenServer.Dapr;

public class DaprEventBusOptions
{
    public const string Position = "Dapr:PubSub";

    public ISet<PubSubComponent> Components { get; set; } = new HashSet<PubSubComponent>()
    {
        new(){ PubSubName = "pubsub"},
        new(){ PubSubName = "pubsub-broadcast", Mode = ConsumeMode.AllConsume },
    };

    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}

public class PubSubComponent
{
    public string PubSubName { get; set; } = string.Empty;

    public ConsumeMode Mode { get; set; }
}