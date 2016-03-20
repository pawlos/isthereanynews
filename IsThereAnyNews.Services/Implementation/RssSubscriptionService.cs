using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Implementation
{
    public class RssSubscriptionService : IRssSubscriptionService
    {
        private readonly ISessionProvider sessionProvider;
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptions;
        private readonly IRssEntriesToReadRepository rssToReadRepository;

        public RssSubscriptionService(ISessionProvider sessionProvider, IRssChannelsSubscriptionsRepository rssSubscriptions, IRssEntriesToReadRepository rssToReadRepository)
        {
            this.sessionProvider = sessionProvider;
            this.rssSubscriptions = rssSubscriptions;
            this.rssToReadRepository = rssToReadRepository;
        }

        public RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(long subscriptionId)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();

            if (!this.rssSubscriptions.DoesUserOwnsSubscription(subscriptionId, currentUserId))
            {
                var rssSubscriptionIndexViewModel = new RssSubscriptionIndexViewModel(new List<RssEntryToRead>());
                rssSubscriptionIndexViewModel.SubscriptionId = subscriptionId;
                return rssSubscriptionIndexViewModel;
            }

            var loadAllRssEntriesForUserAndChannel =
                this.rssToReadRepository.LoadAllUnreadEntriesFromSubscription(subscriptionId);
            var viewModel = new RssSubscriptionIndexViewModel(loadAllRssEntriesForUserAndChannel);
            viewModel.SubscriptionId = subscriptionId;
            return viewModel;
        }

        public void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto dto)
        {
            var separator = new[] { ";" };
            var rssToMarkRead =
                dto.RssEntries.Split(separator, StringSplitOptions.None).ToList().Select(id => long.Parse(id)).ToList();
            this.rssToReadRepository.MarkAllReadForUserAndSubscription(dto.SubscriptionId, rssToMarkRead);

        }
    }
}