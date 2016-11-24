namespace IsThereAnyNews.Automapper
{
    using AutoMapper;

    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ViewModels;

    public class ProjectionToViewModel : Profile
    {
        public ProjectionToViewModel()
        {
            this.CreateMap<ApplicationConfigurationDTO, ItanApplicationConfigurationViewModel>();
        }
    }
}