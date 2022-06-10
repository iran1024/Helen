namespace HelenServer.FileStorage.Contracts
{
    public class StorageNodeModel
    {
        public string GroupName { get; set; } = string.Empty;
        public ConnectionAddressModel ConnectionAddress { get; set; } = null!;
        public byte StorePathIndex { get; set; }
    }
}