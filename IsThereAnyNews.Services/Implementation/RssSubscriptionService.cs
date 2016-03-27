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
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionsRepository;
        private readonly IRssEntriesToReadRepository rssToReadRepository;

        public RssSubscriptionService(ISessionProvider sessionProvider, IRssChannelsSubscriptionsRepository rssSubscriptionsRepository, IRssEntriesToReadRepository rssToReadRepository)
        {
            this.sessionProvider = sessionProvider;
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.rssToReadRepository = rssToReadRepository;
        }

        public RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(long subscriptionId)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();

            if (!this.rssSubscriptionsRepository.DoesUserOwnsSubscription(subscriptionId, currentUserId))
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

        public void MarkEntryViewed(long rssToReadId)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            this.rssToReadRepository.MarkEntryViewedByUser(currentUserId, rssToReadId);
        }

        public void UnsubscribeCurrentUserFromChannelId(long id)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssSubscriptionsRepository.DeleteSubscriptionFromUser(id, userId);

        }

        public void SubscribeCurrentUserToChannel(long channelId)
        {
            var userId = this.sessionProvider.GetCurrentUserId();
            this.rssSubscriptionsRepository.CreateNewSubscriptionForUserAndChannel(userId, channelId);

        }
    }
}