namespace IsThereAnyNews.Automapper
{
    using System.Collections.Generic;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.ViewModels;

    public class EntityToProjectionModels : Profile
    {
        public EntityToProjectionModels()
        {
            var htmlstrip = new HtmlStripper();

            this.CreateMap<ApplicationConfiguration, ApplicationConfigurationDTO>();
            this.CreateMap<RssEntry, RssEntryDTO>();
            this.CreateMap<EventRssUserInteractionDTO, EventRssViewedViewModel>();
            this.CreateMap<RssEntryToRead, RssEntryToReadDTO>();
            this.CreateMap<RssEntryToReadDTO, RssEntryToReadViewModel>()
                .ForMember(d => d.RssEntryViewModel, o => o.MapFrom(s => s.RssEntryDto));

            this.CreateMap<RssEntryDTO, RssEntryViewModel>()
               .ForMember(d => d.PreviewText, o => o.MapFrom(s => htmlstrip.GetContentOnly(s.PreviewText)));

            this.CreateMap<UserSubscriptionEntryToRead, UserSubscriptionEntryToReadDTO>();
            this.CreateMap<RssChannel, RssChannelForUpdateDTO>();
            this.CreateMap<EventRssChannelUpdated, EventRssChannelUpdatedDTO>();
            this.CreateMap<RssChannel, RssChannelDTO>();
            this.CreateMap<RssChannelDTO, RssChannelIndexViewModel>();
            this.CreateMap<User, UserPublicProfileDto>();

            this.CreateMap<User, UserPrivateProfileDto>();
            this.CreateMap<UserPublicProfileDto, UserDetailedPublicProfileViewModel>()
                .ForMember(d => d.ViewingUserId, o => o.MapFrom(s => s.Id));

            this.CreateMap<SocialLogin, SocialLoginDTO>();
            this.CreateMap<SocialLoginDTO, SocialLoginViewModel>();
            this.CreateMap<UserPrivateProfileDto, AccountDetailsViewModel>();
            this.CreateMap<UserSubscription, RssChannelInformationDTO>();
            this.CreateMap<RssChannelSubscription, RssChannelSubscriptionDTO>();
            this.CreateMap<RssChannelSubscription, RssChannelInformationDTO>();
            this.CreateMap<UserSubscriptionEntryToReadDTO, RssEntryToReadViewModel>();
            this.CreateMap<RssChannelSubscriptionDTO, RssChannelSubscriptionViewModel>();

            this.CreateMap<SyndicationItemAdapter, NewRssEntryDTO>()
                .ForMember(d => d.ItemId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ItemTitle, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.ItemUrl, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.ItemSummary, o => o.MapFrom(s => s.Summary))
                .ForMember(d => d.GetContentOnly, o => o.MapFrom(s => htmlstrip.GetContentOnly(s.Summary)))
                .ForMember(d => d.ItemPublishDate, o => o.MapFrom(s => s.PublishDate));

            this.CreateMap<NewRssEntryDTO, RssEntry>()
                .ForMember(d => d.PublicationDate, o => o.MapFrom(s => s.ItemPublishDate))
                .ForMember(d => d.PreviewText, o => o.MapFrom(s => s.ItemSummary))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.ItemTitle))
                .ForMember(d => d.RssChannelId, o => o.MapFrom(s => s.RssChannelId))
                .ForMember(d => d.RssId, o => o.MapFrom(s => s.ItemId))
                .ForMember(d => d.StrippedText, o => o.MapFrom(s => htmlstrip.GetContentOnly(s.ItemSummary)))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.ItemUrl));

            this.CreateMap<List<RssChannelSubscriptionDTO>, RssChannelsMyViewModel>()
                .ForMember(d => d.ChannelsSubscriptions, o => o.MapFrom(s => s))
                .ForMember(d => d.Users, o => o.UseValue(new List<ObservableUserEventsInformation>()));

        }
    }
}
