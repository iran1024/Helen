namespace HelenServer.FileStorage.Contracts
{
    public class ClusterConfigurationModel
    {
        public string Name { get; set; } = string.Empty;
        public List<TrackerModel> Trackers { get; set; } = new List<TrackerModel>();
        public int ConnectionTimeout { get; set; }
        public string Charset { get; set; } = string.Empty;
        public bool AntiStealToken { get; set; }
        public string SecretKey { get; set; } = string.Empty;
        public int ConnectionLifeTime { get; set; }
        public int ConnectionConcurrentThread { get; set; }
        public int ScanTimeoutConnectionInterval { get; set; }
        public int TrackerMaxConnection { get; set; }
        public int StorageMaxConnection { get; set; }
    }
}