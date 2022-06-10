using Google.Protobuf;

namespace HelenServer.Dapr;

public class DaprEventMessageConverter : IEventMessageConverter
{
    private readonly DaprEventBusOptions _options;

    public DaprEventMessageConverter(IOptions<DaprEventBusOptions> options)
    {
        _options = options.Value;
    }

    public TMessage ToMessageModel<TMessage>(ByteString? data)
    {
        if (data is null)
        {
            return default!;
        }

        var message = JsonSerializer.Deserialize<TMessage>(data.ToStringUtf8(), _options.JsonSerializerOptions);

        if (message is null)
        {
            throw new InvalidCastException();
        }

        return message;
    }
}