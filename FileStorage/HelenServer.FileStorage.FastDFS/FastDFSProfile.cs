using AutoMapper;
using FastDFSCore;
using FastDFSCore.Protocols;
using HelenServer.FileStorage.Contracts;

namespace HelenServer.FileStorage.FastDFS
{
    public class FastDFSProfile : Profile
    {
        public FastDFSProfile()
        {
            CreateMap<ClusterConfigurationModel, ClusterConfiguration>().ReverseMap();
            CreateMap<ConnectionAddressModel, ConnectionAddress>().ReverseMap();
            CreateMap<FastDFSFileInfoModel, FastDFSFileInfo>().ReverseMap();
            CreateMap<GroupInfoModel, GroupInfo>().ReverseMap();
            CreateMap<StorageInfoModel, StorageInfo>().ReverseMap();
            CreateMap<StorageNodeModel, StorageNode>().ReverseMap();
            CreateMap<TrackerModel, Tracker>().ReverseMap();
        }
    }
}