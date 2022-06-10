using System.Text.Json.Serialization;

namespace HelenServer.BugEngine.Contracts
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OperationType
    {
        Create = 1,
        Edit,
        Remark,
        Resolve,
        Confirm,
        Active,
        Close
    }
}
