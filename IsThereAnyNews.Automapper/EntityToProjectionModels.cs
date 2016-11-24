namespace IsThereAnyNews.Automapper
{
    using AutoMapper;

    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;

    public class EntityToProjectionModels : Profile
    {
        public EntityToProjectionModels()
        {
            this.CreateMap<ApplicationConfiguration, ApplicationConfigurationDTO>();
        }
    }
}