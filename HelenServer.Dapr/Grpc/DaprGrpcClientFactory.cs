using Dapr.Client;

namespace HelenServer.Dapr;

internal class DaprGrpcClientFactory : IGrpcClientFactory
{
    private readonly DaprClient _daprClient;

    public DaprGrpcClientFactory(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public IGrpcClient<TRequest, TResponse> GetService<TMethod, TRequest, TResponse>(string serviceName)
        where TMethod : IGrpcMethod<TRequest, TResponse>
    {
        if (_daprClient is not DaprClientGrpc grpcClient)
        {
            throw new InvalidOperationException("Espect is gRpcClient");
        }

        if (string.IsNullOrEmpty(serviceName))
        {
            throw new ArgumentException("The value cannot be null or empty", nameof(serviceName));
        }

        return new DaprGrpcClient<TRequest, TResponse>(grpcClient, serviceName, typeof(TMethod).AssemblyQualifiedName!);
    }
}
