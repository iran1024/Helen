namespace HelenServer.Grpc.Contracts
{
    public abstract class GrpcMethod<TReqeust, TResponse> : IGrpcMethod<TReqeust, TResponse>
    {
        public abstract Task<TResponse> InvokeAsync(TReqeust request, ServerCallContext context);

        async Task<object> IGrpcMethod.InvokeAsync(object request, IGrpcConverter converter, ServerCallContext context)
        {
            var model = converter.ToRequest<TReqeust>(request);

            var result = await InvokeAsync(model, context);

            return converter.ToResponse(result);
        }
    }
}