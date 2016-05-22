using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Automapper
{
    public class ModelToViewModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<RssChannelSubscription, RssChannelSubscriptionViewModel>()
                .ForMember(d => d.RssToRead, o => o.MapFrom(s => s.RssEntriesToRead.Count(x => !x.IsRead)));


            CreateMap<List<RssChannelSubscription>, RssChannelsMyViewModel>()
                .ForMember(d => d.ChannelsSubscriptions, o => o.MapFrom(s => s))
                .ForMember(d => d.Users, o => o.UseValue(new List<ObservableUserEventsInformation>()));


            CreateMap<RssEntry, RssEntryViewModel>();

            CreateMap<RssEntryToRead, RssEntryToReadViewModel>()
                .ForMember(d => d.RssEntryViewModel, o => o.MapFrom(s => s.RssEntry));

            CreateMap<UserSubscriptionEntryToRead, RssEntryToReadViewModel>()
                .ForMember(d => d.RssEntryViewModel, o => o.MapFrom(s => s.EventRssViewed.RssEntry));

            CreateMap<List<RssChannelSubscriptionWithStatisticsData>, RssChannelsIndexViewModel>()
                .ForMember(d => d.AllChannels, o => o.MapFrom(s => s));

            CreateMap<RssChannel, RssChannelIndexViewModel>()
                .ForMember(d => d.Entries, o => o.MapFrom(s => s.RssEntries))
                .ForMember(d => d.Added, o => o.MapFrom(s => s.Created))
                .ForMember(d => d.ChannelId, o => o.MapFrom(s => s.Id));

        }
    }
}