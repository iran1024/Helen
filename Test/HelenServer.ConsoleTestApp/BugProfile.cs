using AutoMapper;
using HelenServer.BugEngine.Contracts;

namespace HelenServer.ConsoleTestApp
{
    internal class BugProfile : Profile
    {
        public BugProfile()
        {
            //CreateMap<ZentaoBug, BugModel>().ReverseMap();
            CreateMap<string, ProductModel>().ForMember(dest => dest.Name, opts => opts.MapFrom(src => MapProduct(src))).ReverseMap();
            CreateMap<string, ModuleModel>().ForMember(dest => dest.Name, opts => opts.MapFrom(src => MapModule(src))).ReverseMap();
            CreateMap<string, BugTypeModel>().ForMember(dest => dest.TypeName, opts => opts.MapFrom(src => MapBugType(src))).ReverseMap();
            CreateMap<string, OsModel>().ForMember(dest => dest.Version, opts => opts.MapFrom(src => MapOs(src))).ReverseMap();
            CreateMap<string, BrowserModel>().ForMember(dest => dest.Name, opts => opts.MapFrom(src => MapBrowser(src))).ReverseMap();
            CreateMap<string, BugStatus>().ReverseMap();
            CreateMap<string, BugModel>().ForMember(dest => dest.Id, opts => opts.MapFrom(src => MapRelatedBug(src))).ReverseMap();
        }

        private static ProductModel MapProduct(string src)
        {
            return new ProductModel()
            {
                Name = src
            };
        }

        private static ModuleModel MapModule(string src)
        {
            return new ModuleModel()
            {
                Name = src
            };
        }

        private static BugTypeModel MapBugType(string src)
        {
            return new BugTypeModel()
            {
                TypeName = src
            };
        }

        private static OsModel MapOs(string src)
        {
            return new OsModel()
            {
                Version = src
            };
        }

        private static BrowserModel MapBrowser(string src)
        {
            return new BrowserModel()
            {
                Name = src
            };
        }

        private static BugModel MapRelatedBug(string src)
        {
            return new BugModel()
            {
                Id = int.Parse(src)
            };
        }
    }
}
