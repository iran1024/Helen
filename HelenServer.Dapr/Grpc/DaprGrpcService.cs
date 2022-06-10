using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client.Autogen.Grpc.v1;

namespace HelenServer.Dapr;

public class DaprGrpcService : AppCallback.AppCallbackBase
{
    public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
    {
        if (Type.GetType(request.Method) is Type serviceType)
        {
            var sp = context.GetHttpContext().RequestServices;

            var instance = sp.GetService(serviceType);

            if (instance is IGrpcMethod func)
            {
                var response = await func.InvokeAsync(request, new DaprGrpcConverter(), context);

                return (InvokeResponse)response;
            }
        }

        return await base.OnInvoke(request, context);
    }

    public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(
        Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
    {
        var serviceProvider = context.GetHttpContext().RequestServices;

        var result = new ListTopicSubscriptionsResponse();

        var consumers = serviceProvider.GetServices<IEventConsumer>();

        if (consumers is not null && consumers.Any())
        {
            var options = serviceProvider.GetRequiredService<IOptions<DaprEventBusOptions>>();

            foreach (var item in consumers)
            {
                var publishers = options.Value.Components
                    .Where(p => p.Mode == item.Mode);

                foreach (var publisher in publishers)
                {
                    result.Subscriptions.Add(new TopicSubscription()
                    {
                        PubsubName = publisher.PubSubName,
                        Topic = item.TopicName,
                    });
                }
            }
        }

        return Task.FromResult(result);
    }

    public override async Task<TopicEventResponse> OnTopicEvent(
        TopicEventRequest request, ServerCallContext context)
    {
        var serviceProvider = context.GetHttpContext().RequestServices;

        string topicName = request.Topic;

        var handler = GetEventHandlerService(serviceProvider, topicName);

        if (handler is null)
        {
            return new TopicEventResponse();
        }

        var converter = serviceProvider.GetRequiredService<IEventMessageConverter>();

        var result = await handler.InvokeAsync(request.Data, converter, context);

        if (result is null || !result.Succeed)
        {
            return new TopicEventResponse() { Status = TopicEventResponse.Types.TopicEventResponseStatus.Drop };
        }

        return new TopicEventResponse() { Status = TopicEventResponse.Types.TopicEventResponseStatus.Success };
    }

    private static IEventConsumer? GetEventHandlerService(IServiceProvider provider, string topicName)
    {
        var consumers = provider.GetServices<IEventConsumer>();

        return consumers.FirstOrDefault(p => p.TopicName == topicName);
    }
}

