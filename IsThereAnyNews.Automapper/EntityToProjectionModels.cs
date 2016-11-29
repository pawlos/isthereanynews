namespace IsThereAnyNews.Automapper
{
    using System.Collections.Generic;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ViewModels;

    public class EntityToProjectionModels : Profile
    {
        public EntityToProjectionModels()
        {
            var htmlstrip = new HtmlStripper();

            this.CreateMap<ApplicationConfiguration, ApplicationConfigurationDTO>();
            this.CreateMap<RssEntry, RssEntryDTO>();
            this.CreateMap<RssEntryToRead, RssEntryToReadDTO>();
            this.CreateMap<RssEntryToReadDTO,RssEntryToReadViewModel>();
            this.CreateMap<UserSubscriptionEntryToRead, UserSubscriptionEntryToReadDTO>();
            this.CreateMap<RssChannel, RssChannelForUpdateDTO>();
            this.CreateMap<EventRssChannelUpdated, EventRssChannelUpdatedDTO>();
            this.CreateMap<RssChannel, RssChannelDTO>();
            this.CreateMap<RssChannelDTO, RssChannelIndexViewModel>();
            this.CreateMap<User, UserPublicProfileDto>();
            this.CreateMap<User, UserPrivateProfileDto>();
            this.CreateMap<UserPublicProfileDto, UserDetailedPublicProfileViewModel>();
            this.CreateMap<SocialLogin, SocialLoginDTO>();
            this.CreateMap<SocialLoginDTO, SocialLoginViewModel>();
            this.CreateMap<UserPrivateProfileDto, AccountDetailsViewModel>();
            this.CreateMap<UserSubscription, RssChannelInformationDTO>();
            this.CreateMap<RssChannelSubscription, RssChannelSubscriptionDTO>();
            this.CreateMap<RssChannelSubscription, RssChannelInformationDTO>();
            this.CreateMap<UserSubscriptionEntryToReadDTO, RssEntryToReadViewModel>();
            this.CreateMap<RssChannelSubscriptionDTO, RssChannelSubscriptionViewModel>();
            this.CreateMap<RssEntryDTO, RssEntryViewModel>()
                .ForMember(d => d.PreviewText, o => o.MapFrom(s => htmlstrip.GetContentOnly(s.PreviewText)));
            this.CreateMap<List<RssChannelSubscriptionDTO>, RssChannelsMyViewModel>()
                .ForMember(d => d.ChannelsSubscriptions, o => o.MapFrom(s => s))
                .ForMember(d => d.Users, o => o.UseValue(new List<ObservableUserEventsInformation>()));

        }
    }
}
