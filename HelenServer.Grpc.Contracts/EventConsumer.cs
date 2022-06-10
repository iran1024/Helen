using Google.Protobuf;

namespace HelenServer.Grpc.Contracts;

public abstract class EventConsumer<TMessage> : IEventConsumer<TMessage>
{
    public abstract string TopicName { get; }
    public abstract ConsumeMode Mode { get; }
    public abstract Task<OperationResult> InvokeAsync(TMessage message, ServerCallContext context);

    public Task<OperationResult> InvokeAsync(ByteString data, IEventMessageConverter converter, ServerCallContext context)
    {
        var message = converter.ToMessageModel<TMessage>(data);

        var result = InvokeAsync(message, context);

        return result;
    }
}