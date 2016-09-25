namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class RssSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionsRepository;
        private readonly ISessionProvider sessionProvider;
        private readonly IRssEntriesToReadRepository rssToReadRepository;
        private readonly IMapper mapper;

        private readonly IRssEventRepository rssEventsRepository;

        public RssSubscriptionHandler(
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository,
            ISessionProvider sessionProvider,
            IRssEntriesToReadRepository rssToReadRepository,
            IMapper mapper,
            IRssEventRepository rssEventsRepository)
        {
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.sessionProvider = sessionProvider;
            this.rssToReadRepository = rssToReadRepository;
            this.mapper = mapper;
            this.rssEventsRepository = rssEventsRepository;
        }

        public RssSubscriptionIndexViewModel GetSubscriptionViewModel(long subscriptionId, ShowReadEntries showReadEntries)
        {
            return this.GetRssSubscriptionIndexViewModel(subscriptionId, showReadEntries);
        }

        public void MarkRead(string displayedItems)
        {
            var ids = displayedItems.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => long.Parse(i))
                .ToList();
            this.rssSubscriptionsRepository.MarkRead(ids);
        }

        public void AddEventViewed(long dtoId)
        {
            var cui = this.sessionProvider.GetCurrentUserId();
            this.rssEventsRepository.AddEventRssViewed(cui, dtoId);
        }

        private RssSubscriptionIndexViewModel GetRssSubscriptionIndexViewModel(long subscriptionId, ShowReadEntries showReadEntries)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();

            if (!this.rssSubscriptionsRepository.DoesUserOwnsSubscription(subscriptionId, currentUserId))
            {
                var ci = new ChannelInformationViewModel
                {
                    Title = "You are not subscribed to this channel",
                    Created = DateTime.MaxValue
                };
                var rssSubscriptionIndexViewModel = new RssSubscriptionIndexViewModel(
                                                        subscriptionId,
                                                        ci,
                                                        new List<RssEntryToReadViewModel>(),
                                                        StreamType.Rss);
                return rssSubscriptionIndexViewModel;
            }

            var loadAllRssEntriesForUserAndChannel = showReadEntries != ShowReadEntries.Show
                                                          ? this.rssToReadRepository.LoadAllUnreadEntriesFromSubscription(subscriptionId)
                                                          : this.rssToReadRepository.LoadAllEntriesFromSubscription(subscriptionId);

            var channelInformation = this.rssSubscriptionsRepository.LoadChannelInformation(subscriptionId);
            var channelInformationViewModel = new ChannelInformationViewModel
            {
                Title = channelInformation.Title,
                Created = channelInformation.Created
            };

            var rssEntryToReadViewModels = this.mapper.Map<List<RssEntryToReadViewModel>>(loadAllRssEntriesForUserAndChannel);

            var viewModel = new RssSubscriptionIndexViewModel(
                                subscriptionId,
                                channelInformationViewModel,
                                rssEntryToReadViewModels,
                                StreamType.Rss);
            viewModel.SubscriptionId = subscriptionId;
            return viewModel;
        }
    }
}