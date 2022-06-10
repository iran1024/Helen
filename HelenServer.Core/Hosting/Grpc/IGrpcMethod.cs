namespace HelenServer.Core;

public interface IGrpcMethod
{
    Task<object> InvokeAsync(object request, IGrpcConverter converter, ServerCallContext context);
}

public interface IGrpcMethod<TRequest, TResponse> : IGrpcMethod
{
    Task<TResponse> InvokeAsync(TRequest request, ServerCallContext context);
}

public interface IGrpcConverter
{
    TRequest ToRequest<TRequest>(object request);
    object ToResponse<TResponse>(TResponse response);
}
