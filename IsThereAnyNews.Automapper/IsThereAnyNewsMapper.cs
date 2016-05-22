using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Automapper
{
    public class IsThereAnyNewsMapper
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(config =>
                config.AddProfile(new ModelToViewModel()));
        }
    }

    public class ModelToViewModel : Profile
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


        }
    }
}