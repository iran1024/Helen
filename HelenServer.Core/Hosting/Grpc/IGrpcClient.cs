namespace HelenServer.Core;

public interface IGrpcClient<TRequest, TResponse>
{
    Task<TResponse> InvokeAsync(TRequest request, Metadata headers, CancellationToken cancellationToken);
}
