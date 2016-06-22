using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.SharedData;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Implementation
{
    public class RssSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionsRepository;
        private readonly ISessionProvider sessionProvider;
        private readonly IRssEntriesToReadRepository rssToReadRepository;
        private readonly IMapper mapper;

        public RssSubscriptionHandler(
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository,
            ISessionProvider sessionProvider, 
            IRssEntriesToReadRepository rssToReadRepository, 
            IMapper mapper)
        {
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.sessionProvider = sessionProvider;
            this.rssToReadRepository = rssToReadRepository;
            this.mapper = mapper;
        }

        public RssSubscriptionIndexViewModel GetSubscriptionViewModel(long subscriptionId, ShowReadEntries showReadEntries)
        {
            return GetRssSubscriptionIndexViewModel(subscriptionId, showReadEntries);
        }

        public void MarkRead(string displayedItems)
        {
            var ids = displayedItems.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => long.Parse(i))
                .ToList();
            this.rssSubscriptionsRepository.MarkRead(ids);
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
                var rssSubscriptionIndexViewModel = new RssSubscriptionIndexViewModel(subscriptionId, ci,
                    new List<RssEntryToReadViewModel>(),
                    StreamType.Rss);
                return rssSubscriptionIndexViewModel;
            }

            List<RssEntryToRead> loadAllRssEntriesForUserAndChannel = null;
            if (showReadEntries != ShowReadEntries.Show)
            {
                loadAllRssEntriesForUserAndChannel =
                    this.rssToReadRepository.LoadAllUnreadEntriesFromSubscription(subscriptionId);
            }
            else
            {
                loadAllRssEntriesForUserAndChannel =
                    this.rssToReadRepository.LoadAllEntriesFromSubscription(subscriptionId);
            }
            var channelInformation = this.rssSubscriptionsRepository.LoadChannelInformation(subscriptionId);
            var channelInformationViewModel = new ChannelInformationViewModel
            {
                Title = channelInformation.Title,
                Created = channelInformation.Created
            };

            var rssEntryToReadViewModels = this.mapper.Map<List<RssEntryToReadViewModel>>(loadAllRssEntriesForUserAndChannel);

            var viewModel = new RssSubscriptionIndexViewModel(subscriptionId,
                channelInformationViewModel,
                rssEntryToReadViewModels, StreamType.Rss);
            viewModel.SubscriptionId = subscriptionId;
            return viewModel;
        }
    }
}