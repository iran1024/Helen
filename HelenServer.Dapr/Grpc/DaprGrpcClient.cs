using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;

namespace HelenServer.Dapr;

internal class DaprGrpcClient<TRequest, TResponse> : IGrpcClient<TRequest, TResponse>
{
    private readonly string _serviceName;
    private readonly string _methodName;
    private readonly DaprClientGrpc _grpcClient;

    public DaprGrpcClient(DaprClientGrpc grpcClient, string serviceName, string methodName)
    {
        _serviceName = serviceName;
        _methodName = methodName;
        _grpcClient = grpcClient;
    }

    public async Task<TResponse> InvokeAsync(TRequest request, Metadata headers, CancellationToken cancellationToken)
    {
        var requestMessage = new InvokeServiceRequest
        {
            Id = _serviceName,
            Message = new InvokeRequest
            {
                ContentType = "application/grpc",
                Data = GrpcUtilities.ToAny(request),
                Method = _methodName,
            },
        };

        var options = new CallOptions(headers, cancellationToken: cancellationToken);

        try
        {
            var response = await _grpcClient.Client.InvokeServiceAsync(requestMessage, options);

            var result = GrpcUtilities.ToModel<TResponse>(response.Data);

            return result;
        }
        catch (RpcException ex)
        {
            throw new InvocationException(requestMessage.Id, requestMessage.Message.Method, ex);
        }
    }
}
