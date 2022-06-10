namespace HelenServer.FileStorage.Contracts
{
    public class GroupInfoModel
    {
        public string GroupName { get; set; } = string.Empty;
        public long TotalMB { get; set; }
        public long FreeMB { get; set; }
        public long TrunkFreeMb { get; set; }
        public int ServerCount { get; set; }
        public int StoragePort { get; set; }
        public int StorageHttpPort { get; set; }
        public int ActiveCount { get; set; }
        public int CurrentWriteServerIndex { get; set; }
        public int StorePathCount { get; set; }
        public int SubdirCount { get; set; }
        public long CurrentTrunkFileId { get; set; }
    }
}