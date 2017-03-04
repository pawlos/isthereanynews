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
                        cfg.AddProfile<AutomapperProfiles>();
                    });

            return Mapper.Instance;
        }
    }
}