namespace HelenServer.FileStorage.Contracts
{
    public class FastDFSFileInfoModel
    {
        public long FileSize { get; set; }
        public DateTime CreateTime { get; set; }
        public long Crc32 { get; set; }
    }
}