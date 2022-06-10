namespace HelenServer.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderByType
    {
        ASC,
        DESC
    }
}