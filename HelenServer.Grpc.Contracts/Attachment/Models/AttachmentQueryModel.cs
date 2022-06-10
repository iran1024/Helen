using System.Runtime.Serialization;

namespace HelenServer.Grpc.Contracts
{
    [DataContract]
    public class AttachmentQueryModel
    {
        [DataMember(Order = 1)]
        public string AttachmentNo { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public string ProviderId { get; set; } = string.Empty;
    }
}