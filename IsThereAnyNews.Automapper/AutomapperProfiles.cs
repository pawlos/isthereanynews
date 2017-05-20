using System.ServiceModel.Syndication;
using IsThereAnyNews.ViewModels.Subscriptions;

namespace IsThereAnyNews.Automapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;
    using IsThereAnyNews.ViewModels.RssChannel;

    public class AutomapperProfiles: Profile
    {
        public AutomapperProfiles()
        {
            var htmlstrip = new HtmlStripper();
            this.CreateMap<ApplicationConfigurationDTO, ItanApplicationConfigurationViewModel>();
            this.CreateMap<FeedEntries, FeedEntriesViewModel>()
                .ForMember(d => d.RssEntryViewModels, o => o.MapFrom(s => s.RssEntryDtos));

            this.CreateMap<SyndicationItem, SyndicationItemAdapter>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
              .ForMember(d => d.PublishDate, o => o.MapFrom(s => s.PublishDate.UtcDateTime))
              .ForMember(d => d.Summary, o => o.ResolveUsing<SyndicationSummaryResolver>())
              .ForMember(d => d.Title, o => o.MapFrom(s => s.Title.Text))
              .ForMember(d => d.Url, o => o.ResolveUsing<SyndicationUrlResolver>());

            this.CreateMap<RssChannelSubscription, RssChannelSubscriptionViewModel>()
                .ForMember(d => d.Count, o => o.MapFrom(s => s.RssEntriesToRead.Count(x => !x.IsRead)));

            this.CreateMap<List<RssChannelSubscription>, RssChannelsMyViewModel>()
                .ForMember(d => d.ChannelsSubscriptions, o => o.MapFrom(s => s))
                .ForMember(d => d.Users, o => o.UseValue(new List<ObservableUserEventsInformation>()));

            this.CreateMap<RssEntry, RssEntryViewModel>()
                .ForMember(d => d.PreviewText, o => o.MapFrom(s => htmlstrip.GetContentOnly(s.PreviewText)));

            this.CreateMap<RssEntryToRead, RssEntryToReadViewModel>()
                .ForMember(d => d.RssEntryViewModel, o => o.MapFrom(s => s.RssEntry));

            this.CreateMap<UserSubscriptionEntryToRead, RssEntryToReadViewModel>()
                .ForMember(d => d.RssEntryViewModel, o => o.MapFrom(s => s.EventRssUserInteraction.RssEntry));

            this.CreateMap<RssChannelSubscriptionWithStatisticsData, RssChannelWithStatisticsViewModel>();

            this.CreateMap<List<RssChannelSubscriptionWithStatisticsData>, RssChannelsIndexViewModel>()
                .ForMember(d => d.AllChannels, o => o.MapFrom(s => s));

            this.CreateMap<RssChannel, RssChannelIndexViewModel>()
                .ForMember(d => d.Entries, o => o.MapFrom(s => s.RssEntries))
                .ForMember(d => d.Added, o => o.MapFrom(s => s.Created))
                .ForMember(d => d.ChannelId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Updated, o => o.ResolveUsing<UpdateResolver>());
            this.CreateMap<long, UserRssSubscriptionInfoViewModel>()
                .ForMember(d => d.ChannelSubscriptionId, o => o.MapFrom(s => s))
                .ForMember(d => d.IsSubscribed, o => o.Ignore());

            this.CreateMap<ItanRole, Claim>()
                .ConstructUsing(this.CreateClaim);

            this.CreateMap<User, AccountDetailsViewModel>()
                .ForMember(d => d.Registered, o => o.MapFrom(s => s.Created))
                .ForMember(d => d.SocialLogins, o => o.MapFrom(s => s.SocialLogins));

            this.CreateMap<SocialLogin, SocialLoginViewModel>()
                .ForMember(d => d.AuthenticationTypeProvider, o => o.MapFrom(s => s.Provider))
                .ForMember(d => d.Registered, o => o.MapFrom(s => s.Created));

            this.CreateMap<User, UserDetailedPublicProfileViewModel>()
                .ForMember(d => d.ChannelsCount, o => o.MapFrom(s => s.RssSubscriptionList.Count))
                .ForMember(d => d.EventsCount, o => o.MapFrom(s => s.EventsRssViewed.Count))
                .ForMember(d => d.Events, o => o.MapFrom(s => s.EventsRssViewed))
                .ForMember(d => d.Channels, o => o.MapFrom(s => s.RssSubscriptionList))
                .ForMember(d => d.ViewingUserId, o => o.MapFrom(s => s.Id));

            this.CreateMap<NameAndCountUserSubscription, PublicProfileChannelInformation>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.DisplayName))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.UserId));

            this.CreateMap<RssChannelSubscription, PublicProfileChannelInformation>()
               .ForMember(d => d.Name, o => o.MapFrom(s => s.Title));

            this.CreateMap<EventRssUserInteraction, EventRssViewedViewModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.RssEntry.Title))
                .ForMember(d => d.Viewed, o => o.MapFrom(s => s.Created))
                .ForMember(d => d.RssId, o => o.MapFrom(s => s.Id));

            this.CreateMap<List<RssChannelUpdatedEvent>, List<ISubscriptionViewModel>>();

            this.CreateMap<AddChannelDto, RssChannel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.RssChannelName))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.RssChannelLink))
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.RssEntries, o => o.Ignore())
                .ForMember(d => d.RssLastUpdatedTime, o => o.Ignore())
                .ForMember(d => d.Subscriptions, o => o.Ignore())
                .ForMember(d => d.Updated, o => o.Ignore());

            this.CreateMap<ContactAdministrationDto, ContactAdministration>();

            this.CreateMap<ContactAdministration, ContactAdministrationEvent>()
                .ForMember(d => d.ContactAdministrationId, o => o.MapFrom(s => s.Id));

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
            this.CreateMap<UserSubscriptionEntryToReadDTO, RssEntryToReadViewModel>()
                .ForMember(d => d.RssEntryViewModel, o => o.MapFrom(s => s.RssEntryDto));
            this.CreateMap<RssChannelSubscriptionDTO, RssChannelSubscriptionViewModel>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Title));

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

        private Claim CreateClaim(ItanRole role)
        {
            return new Claim(ClaimTypes.Role, Enum.GetName(typeof(ItanRole), role));
        }
    }
}
