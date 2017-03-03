namespace IsThereAnyNews.Automapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;
    using IsThereAnyNews.ViewModels.RssChannel;

    public class ModelToViewModelProfile : Profile
    {
        public ModelToViewModelProfile()
        {
            var htmlstrip = new HtmlStripper();
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

            this.CreateMap<Dtos.RssChannelSubscriptionWithStatisticsData, RssChannelWithStatisticsViewModel>();

            this.CreateMap<List<Dtos.RssChannelSubscriptionWithStatisticsData>, RssChannelsIndexViewModel>()
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

            this.CreateMap<Dtos.NameAndCountUserSubscription, PublicProfileChannelInformation>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));

            this.CreateMap<RssChannelSubscription, PublicProfileChannelInformation>()
               .ForMember(d => d.Name, o => o.MapFrom(s => s.Title));

            this.CreateMap<EventRssUserInteraction, EventRssViewedViewModel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.RssEntry.Title))
                .ForMember(d => d.Viewed, o => o.MapFrom(s => s.Created))
                .ForMember(d => d.RssId, o => o.MapFrom(s => s.Id));

            this.CreateMap<List<RssChannelUpdatedEvent>, List<ChannelEventViewModel>>();

        }

        private Claim CreateClaim(ItanRole role)
        {
            return new Claim(ClaimTypes.Role, Enum.GetName(typeof(ItanRole), role));
        }
    }
}