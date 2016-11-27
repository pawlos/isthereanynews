namespace IsThereAnyNews.Automapper
{
    using AutoMapper;

    public static class IsThereAnyNewsAutomapper
    {
        public static IMapper ConfigureMapper()
        {
            Mapper.Initialize(
                cfg =>
                    {
                        cfg.AddProfile<ModelToViewModelProfile>();
                        cfg.AddProfile<DtoToEntityModelProfile>();
                        cfg.AddProfile<SyndicationToAdapter>();
                        cfg.AddProfile<EntityToProjectionModels>();
                        cfg.AddProfile<ProjectionToViewModel>();
                    });

            return Mapper.Instance;
        }
    }
}