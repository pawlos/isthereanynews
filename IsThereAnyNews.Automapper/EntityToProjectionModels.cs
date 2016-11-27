namespace IsThereAnyNews.Automapper
{
    using AutoMapper;

    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ViewModels;

    public class EntityToProjectionModels : Profile
    {
        public EntityToProjectionModels()
        {
            this.CreateMap<ApplicationConfiguration, ApplicationConfigurationDTO>();
            this.CreateMap<RssEntry, RssEntryDTO>();
            this.CreateMap<RssEntryToRead, RssEntryToReadDTO>();
            this.CreateMap<UserSubscriptionEntryToRead, UserSubscriptionEntryToReadDTO>();
            this.CreateMap<RssChannel, RssChannelForUpdateDTO>();
            this.CreateMap<EventRssChannelUpdated, EventRssChannelUpdatedDTO>();
            this.CreateMap<RssChannel, RssChannelDTO>();
            this.CreateMap<RssChannelDTO, RssChannelIndexViewModel>();
            this.CreateMap<User, UserPublicProfileDto>();
            this.CreateMap<UserPublicProfileDto, UserDetailedPublicProfileViewModel>();
        }
    }
}
