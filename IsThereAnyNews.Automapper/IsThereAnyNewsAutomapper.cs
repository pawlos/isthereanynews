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
                cfg.AddProfile<EntityToProjectionModels>();
                cfg.AddProfile<ProjectionToViewModel>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}