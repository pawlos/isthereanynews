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
        private readonly IRssChannelsRepository rssChannelsRepository;
        private readonly ISubscriptionHandlerFactory subscriptionHandlerFactory;

        public RssSubscriptionService(
            ISessionProvider sessionProvider,
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository,
            IRssEntriesToReadRepository rssToReadRepository,
            IRssEventRepository rssEventsRepository,
            IRssChannelsRepository rssChannelsRepository,
            ISubscriptionHandlerFactory subscriptionHandlerFactory)
        {
            this.sessionProvider = sessionProvider;
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.rssToReadRepository = rssToReadRepository;
            this.rssEventsRepository = rssEventsRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.subscriptionHandlerFactory = subscriptionHandlerFactory;
        }

        public RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(StreamType streamType, long subscriptionId, ShowReadEntries showReadEntries)
        {
            var provider = subscriptionHandlerFactory.GetProvider(streamType);
            var viewmodel = provider.GetSubscriptionViewModel(subscriptionId, showReadEntries);
            return viewmodel;
        }

        public void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto dto)
        {
            var separator = new[] { ";" };
            var rssToMarkRead =
                dto.RssEntries.Split(separator, StringSplitOptions.None)
                .ToList()
                .Select(id => long.Parse(id)).ToList();
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
            var rssChannel = this.rssChannelsRepository.Load(channelId);
            var addChannelDto = new AddChannelDto
            {
                RssChannelLink = rssChannel.Url,
                RssChannelName = rssChannel.Title
            };
            this.SubscribeCurrentUserToChannel(addChannelDto);
        }

        public void MarkRead(MarkReadDto dto)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(dto.StreamType);
            subscriptionHandler.MarkRead(dto.DisplayedItems);
        }

        public void SubscribeCurrentUserToChannel(AddChannelDto channelId)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            var isUserSubscribedToChannelUrl = this.rssSubscriptionsRepository.IsUserSubscribedToChannelUrl(currentUserId, channelId.RssChannelLink);

            if (!isUserSubscribedToChannelUrl)
            {
                var idByChannelUrl = this.rssChannelsRepository
                    .GetIdByChannelUrl(new List<string> { channelId.RssChannelLink })
                    .Single();

                var rssChannelSubscription = new RssChannelSubscription
                {
                    RssChannelId = idByChannelUrl,
                    UserId = currentUserId,
                    Title = channelId.RssChannelName
                };

                this.rssSubscriptionsRepository.SaveToDatabase(new List<RssChannelSubscription> { rssChannelSubscription });
            }
        }

        public void MarkEntryClicked(MarkClickedDto dto)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            this.rssEventsRepository.MarkClicked(dto.Id, currentUserId);
        }
    }
}