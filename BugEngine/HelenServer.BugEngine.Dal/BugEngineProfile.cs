using AutoMapper;
using HelenServer.BugEngine.Contracts;

namespace HelenServer.BugEngine.Dal
{
    public class BugEngineProfile : Profile
    {
        public BugEngineProfile()
        {
            CreateMap<Browser, BrowserModel>().ReverseMap();
            CreateMap<Bug, BugModel>().ReverseMap();
            CreateMap<BugType, BugTypeModel>().ReverseMap();
            CreateMap<Module, ModuleModel>().ReverseMap();
            CreateMap<OperateLog, OperationLogModel>().ReverseMap();
            CreateMap<Os, OsModel>().ReverseMap();
            CreateMap<Product, ProductModel>().ReverseMap();
            CreateMap<PublishVersion, PublishVersionModel>().ReverseMap();
        }
    }
}