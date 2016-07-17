namespace IsThereAnyNews.Automapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;

    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class ModelToViewModelProfile : Profile
    {
        public ModelToViewModelProfile()
        {
            this.CreateMap<RssChannelSubscription, RssChannelSubscriptionViewModel>()
                .ForMember(d => d.RssToRead, o => o.MapFrom(s => s.RssEntriesToRead.Count(x => !x.IsRead)));

            this.CreateMap<List<RssChannelSubscription>, RssChannelsMyViewModel>()
                .ForMember(d => d.ChannelsSubscriptions, o => o.MapFrom(s => s))
                .ForMember(d => d.Users, o => o.UseValue(new List<ObservableUserEventsInformation>()));

            this.CreateMap<RssEntry, RssEntryViewModel>();

            this.CreateMap<RssEntryToRead, RssEntryToReadViewModel>()
                .ForMember(d => d.RssEntryViewModel, o => o.MapFrom(s => s.RssEntry));

            this.CreateMap<UserSubscriptionEntryToRead, RssEntryToReadViewModel>()
                .ForMember(d => d.RssEntryViewModel, o => o.MapFrom(s => s.EventRssViewed.RssEntry));

            this.CreateMap<RssChannelSubscriptionWithStatisticsData, RssChannelWithStatisticsViewModel>();

            this.CreateMap<List<RssChannelSubscriptionWithStatisticsData>, RssChannelsIndexViewModel>()
                .ForMember(d => d.AllChannels, o => o.MapFrom(s => s));

            this.CreateMap<RssChannel, RssChannelIndexViewModel>()
                .ForMember(d => d.Entries, o => o.MapFrom(s => s.RssEntries))
                .ForMember(d => d.Added, o => o.MapFrom(s => s.Created))
                .ForMember(d => d.ChannelId, o => o.MapFrom(s => s.Id));

            this.CreateMap<long, UserRssSubscriptionInfoViewModel>()
                .ForMember(d => d.ChannelSubscriptionId, o => o.MapFrom(s => s))
                .ForMember(d => d.IsSubscribed, o => o.Ignore());

            this.CreateMap<UserRole, Claim>()
                .ConstructUsing(this.CreateClaim);

            this.CreateMap<User, AccountDetailsViewModel>()
                .ForMember(d => d.Registered, o => o.MapFrom(s => s.Created))
                .ForMember(d => d.SocialLogins, o => o.MapFrom(s => s.SocialLogins));

            this.CreateMap<SocialLogin, SocialLoginViewModel>()
                .ForMember(d => d.AuthenticationTypeProvider, o => o.MapFrom(s => s.Provider))
                .ForMember(d => d.Registered, o => o.MapFrom(s => s.Created));
        }

        private Claim CreateClaim(UserRole role)
        {
            return new Claim(ClaimTypes.Role, Enum.GetName(typeof(ItanRole), role.RoleType));
        }
    }
}