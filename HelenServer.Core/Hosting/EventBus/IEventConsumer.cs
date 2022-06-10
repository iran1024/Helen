using Google.Protobuf;

namespace HelenServer.Core;

public interface IEventConsumer
{
    abstract string TopicName { get; }
    abstract ConsumeMode Mode { get; }
    Task<OperationResult> InvokeAsync(ByteString data, IEventMessageConverter converter, ServerCallContext context);
}

public interface IEventConsumer<in TMessage> : IEventConsumer
{
    Task<OperationResult> InvokeAsync(TMessage message, ServerCallContext context);
}