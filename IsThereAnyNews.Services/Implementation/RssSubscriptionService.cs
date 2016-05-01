using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.SharedData;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Implementation
{
    public class RssSubscriptionService : IRssSubscriptionService
    {
        private readonly ISessionProvider sessionProvider;
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionsRepository;
        private readonly IRssEntriesToReadRepository rssToReadRepository;
        private readonly IRssEventRepository rssEventsRepository;
        private readonly IUsersSubscriptionRepository usersSubscriptionRepository;

        public RssSubscriptionService(
            ISessionProvider sessionProvider,
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository,
            IRssEntriesToReadRepository rssToReadRepository,
            IRssEventRepository rssEventsRepository,
            IUsersSubscriptionRepository usersSubscriptionRepository)
        {
            this.sessionProvider = sessionProvider;
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.rssToReadRepository = rssToReadRepository;
            this.rssEventsRepository = rssEventsRepository;
            this.usersSubscriptionRepository = usersSubscriptionRepository;
        }


        public RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(StreamType streamType, long subscriptionId)
        {
            switch (streamType)
            {
                case StreamType.Rss:
                    return GetRssSubscriptionIndexViewModel(subscriptionId);
                case StreamType.Person:
                    return GetPersonSubscriptionIndexViewModel(subscriptionId);
                default:
                    throw new ArgumentOutOfRangeException(nameof(streamType), streamType, null);
            }
        }

        private RssSubscriptionIndexViewModel GetPersonSubscriptionIndexViewModel(long subscriptionId)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();

            if (!this.usersSubscriptionRepository.DoesUserOwnsSubscription(subscriptionId, currentUserId))
            {
                var ci = new ChannelInformationViewModel
                {
                    Title = "You are not subscribed to this user",
                    Created = DateTime.MaxValue
                };

                var rssSubscriptionIndexViewModel = new RssSubscriptionIndexViewModel(ci, new List<RssEntryToRead>());
                rssSubscriptionIndexViewModel.SubscriptionId = subscriptionId;
                return rssSubscriptionIndexViewModel;
            }

            var loadAllUnreadEntriesFromSubscription = this.usersSubscriptionRepository
                .LoadAllUnreadEntriesFromSubscription(subscriptionId);


            var channelInformation =
                this.usersSubscriptionRepository
                    .LoadChannelInformation(subscriptionId);

            var channelInformationViewModel = new ChannelInformationViewModel
            {
                Title = channelInformation.Title,
                Created = channelInformation.Created
            };

            var viewModel = new RssSubscriptionIndexViewModel(
                    channelInformationViewModel, loadAllUnreadEntriesFromSubscription
                    );

            viewModel.SubscriptionId = subscriptionId;
            return viewModel;
        }

        private RssSubscriptionIndexViewModel GetRssSubscriptionIndexViewModel(long subscriptionId)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();

            if (!this.rssSubscriptionsRepository.DoesUserOwnsSubscription(subscriptionId, currentUserId))
            {
                var ci = new ChannelInformationViewModel
                {
                    Title = "You are not subscribed to this channel",
                    Created = DateTime.MaxValue
                };
                var rssSubscriptionIndexViewModel = new RssSubscriptionIndexViewModel(ci, new List<RssEntryToRead>());
                rssSubscriptionIndexViewModel.SubscriptionId = subscriptionId;
                return rssSubscriptionIndexViewModel;
            }

            var loadAllRssEntriesForUserAndChannel =
                this.rssToReadRepository.LoadAllUnreadEntriesFromSubscription(subscriptionId);

            var channelInformation = this.rssSubscriptionsRepository.LoadChannelInformation(subscriptionId);
            var channelInformationViewModel = new ChannelInformationViewModel
            {
                Title = channelInformation.Title,
                Created = channelInformation.Created
            };

            var viewModel = new RssSubscriptionIndexViewModel(channelInformationViewModel, loadAllRssEntriesForUserAndChannel);
            viewModel.SubscriptionId = subscriptionId;
            return viewModel;
        }



        public void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto dto)
        {
            var separator = new[] { ";" };
            var rssToMarkRead = dto.RssEntries.Split(separator, StringSplitOptions.None).ToList().Select(id => long.Parse(id)).ToList();
            this.rssToReadRepository.MarkAllReadForUserAndSubscription(dto.SubscriptionId, rssToMarkRead);
        }

        public void MarkEntryViewed(long rssToReadId)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            this.rssToReadRepository.MarkEntryViewedByUser(currentUserId, rssToReadId);
            this.rssEventsRepository.AddEventRssViewed(currentUserId, rssToReadId);
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