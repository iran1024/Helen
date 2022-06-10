namespace HelenServer.Grpc.Contracts
{
    public interface IASGetFile : IGrpcMethod<string, GrpcResult<Stream>>
    {

    }
}