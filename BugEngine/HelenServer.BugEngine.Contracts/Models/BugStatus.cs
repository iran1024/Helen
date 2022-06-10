using System.Text.Json.Serialization;

namespace HelenServer.BugEngine.Contracts
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BugStatus
    {
        Active = 1,
        Resolved = 2,
        Closed = 3
    }
}