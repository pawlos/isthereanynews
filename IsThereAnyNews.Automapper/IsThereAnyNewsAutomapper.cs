namespace IsThereAnyNews.Automapper
{
    using AutoMapper;

    public static class IsThereAnyNewsAutomapper
    {
        public static IMapper ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelToViewModelProfile>();
                cfg.AddProfile<DtoToEntityModelProfile>();
                cfg.AddProfile<SyndicationToAdapter>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}