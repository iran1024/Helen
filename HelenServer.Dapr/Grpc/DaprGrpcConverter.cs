using Dapr.Client.Autogen.Grpc.v1;

namespace HelenServer.Dapr
{
    internal class DaprGrpcConverter : IGrpcConverter
    {
        public TRequest ToRequest<TRequest>(object request)
        {
            var data = ((InvokeRequest)request).Data;

            var model = GrpcUtilities.ToModel<TRequest>(data);

            return model;
        }

        public object ToResponse<TResponse>(TResponse response)
        {
            var result = new InvokeResponse
            {
                Data = GrpcUtilities.ToAny(response)
            };

            return result;
        }
    }
}
