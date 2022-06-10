namespace HelenServer.FileStorage.Contracts
{
    public class StorageInfoModel
    {
        public long SuccessDownloadCount { get; set; }
        public long TotalGetMetaCount { get; set; }
        public long SuccessGetMetaCount { get; set; }
        public long TotalCreateLinkCount { get; set; }
        public long SuccessCreateLinkCount { get; set; }
        public long TotalDeleteLinkCount { get; set; }
        public long SuccessDeleteLinkCount { get; set; }
        public long TotalUploadBytes { get; set; }
        public long SuccessUploadBytes { get; set; }
        public long TotalAppendBytes { get; set; }
        public long SuccessAppendBytes { get; set; }
        public long TotalModifyBytes { get; set; }
        public long SuccessModifyBytes { get; set; }
        public long TotalDownloadCount { get; set; }
        public long TotalDownloadBytes { get; set; }
        public long TotalSyncInBytes { get; set; }
        public long SuccessSyncInBytes { get; set; }
        public long TotalSyncOutBytes { get; set; }
        public long SuccessSyncOutBytes { get; set; }
        public long TotalFileOpenCount { get; set; }
        public long SuccessFileOpenCount { get; set; }
        public long TotalFileReadCount { get; set; }
        public long SuccessFileReadCount { get; set; }
        public long TotalFileWriteCount { get; set; }
        public long SuccessFileWriteCount { get; set; }
        public DateTime LastSourceUpdate { get; set; }
        public DateTime LastSyncUpdate { get; set; }
        public DateTime LastSyncedTimestamp { get; set; }
        public long SuccessDownloadBytes { get; set; }
        public DateTime LastHeartbeatTime { get; set; }
        public long SuccessDeleteCount { get; set; }
        public long SuccessSetMetaCount { get; set; }
        public byte Status { get; set; }
        public string StorageId { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string SrcIPAddress { get; set; } = string.Empty;
        public string DomainName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public DateTime JoinTime { get; set; }
        public DateTime UpTime { get; set; }
        public long TotalMb { get; set; }
        public long FreeMb { get; set; }
        public int UploadPriority { get; set; }
        public int StorePathCount { get; set; }
        public int SubdirCount { get; set; }
        public long TotalDeleteCount { get; set; }
        public int CurrentWritePath { get; set; }
        public int StorageHttpPort { get; set; }
        public int AllocCount { get; set; }
        public int CurrentCount { get; set; }
        public int MaxCount { get; set; }
        public long TotalUploadCount { get; set; }
        public long SuccessUploadCount { get; set; }
        public long TotalAppendCount { get; set; }
        public long SuccessAppendCount { get; set; }
        public long TotalModifyCount { get; set; }
        public long SuccessModifyCount { get; set; }
        public long TotalTruncateCount { get; set; }
        public long SuccessTruncateCount { get; set; }
        public long TotalSetMetaCount { get; set; }
        public int StoragePort { get; set; }
        public bool IsTrunkServer { get; set; }
    }
}