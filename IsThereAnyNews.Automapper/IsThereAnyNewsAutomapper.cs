using AutoMapper;

namespace IsThereAnyNews.Automapper
{
    public static class IsThereAnyNewsAutomapper
    {
        public static IMapper ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelToViewModelProfile>();
                cfg.AddProfile<DtoToEntityModelProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}