using System.Runtime.Serialization;

namespace HelenServer.Grpc.Contracts;

[DataContract]
public class GrpcResult
{
    [DataMember(Order = 1)]
    public string Message { get; set; } = string.Empty;

    [DataMember(Order = 2)]
    public OperationStatus Status { get; set; }

    public bool IsSuccess => Status == OperationStatus.Success;
}

[DataContract]
public class GrpcResult<T>
{
    [DataMember(Order = 1)]
    public string Message { get; set; } = string.Empty;

    [DataMember(Order = 2)]
    public OperationStatus Status { get; set; }

    public bool IsSuccess => Status == OperationStatus.Success;

    [DataMember(Order = 3)]
    public T Data { get; set; } = default!;
}