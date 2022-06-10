namespace HelenServer.Core;

public interface IGrpcClientFactory
{
    IGrpcClient<TRequest, TResponse> GetService<TMethod, TRequest, TResponse>(string serviceName)
        where TMethod : IGrpcMethod<TRequest, TResponse>;
}
