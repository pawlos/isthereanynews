namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class RssSubscriptionService : IRssSubscriptionService
    {
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionsRepository;
        private readonly IRssEntriesToReadRepository rssToReadRepository;
        private readonly IRssEventRepository rssEventsRepository;
        private readonly IRssChannelsRepository rssChannelsRepository;
        private readonly ISubscriptionHandlerFactory subscriptionHandlerFactory;

        private readonly IUserAuthentication authentication;

        public RssSubscriptionService(
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository,
            IRssEntriesToReadRepository rssToReadRepository,
            IRssEventRepository rssEventsRepository,
            IRssChannelsRepository rssChannelsRepository,
            ISubscriptionHandlerFactory subscriptionHandlerFactory,
            IUserAuthentication authentication)
        {
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.rssToReadRepository = rssToReadRepository;
            this.rssEventsRepository = rssEventsRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.subscriptionHandlerFactory = subscriptionHandlerFactory;
            this.authentication = authentication;
        }

        public RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(StreamType streamType, long subscriptionId, ShowReadEntries showReadEntries)
        {
            var provider = this.subscriptionHandlerFactory.GetProvider(streamType);
            var viewmodel = provider.GetSubscriptionViewModel(subscriptionId, showReadEntries);
            return viewmodel;
        }

        public void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto dto)
        {
            var separator = new[] { ";" };
            var rssToMarkRead =
                dto.RssEntries.Split(separator, StringSplitOptions.None)
                .Select(long.Parse)
                .ToList();
            this.rssToReadRepository.MarkAllReadForUserAndSubscription(dto.SubscriptionId, rssToMarkRead);
        }

        public void UnsubscribeCurrentUserFromChannelId(long id)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.rssSubscriptionsRepository.DeleteSubscriptionFromUser(id, userId);
        }

        public void SubscribeCurrentUserToChannel(long channelId)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            var isUserSubscribedToChannelUrl = this.rssSubscriptionsRepository.IsUserSubscribedToChannelId(currentUserId, channelId);

            if (!isUserSubscribedToChannelUrl)
            {
                this.rssSubscriptionsRepository.Subscribe(channelId, currentUserId);
            }
        }

        public void SubscribeCurrentUserToChannel(AddChannelDto channelId)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            var isUserSubscribedToChannelUrl = this.rssSubscriptionsRepository.IsUserSubscribedToChannelUrl(currentUserId, channelId.RssChannelLink);

            if (!isUserSubscribedToChannelUrl)
            {
                var idByChannelUrl = this.rssChannelsRepository
                    .GetIdByChannelUrl(new List<string> { channelId.RssChannelLink })
                    .Single();

                this.rssSubscriptionsRepository.Subscribe(idByChannelUrl, currentUserId, channelId.RssChannelName);
            }
        }

        public void MarkRead(MarkReadDto dto)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(dto.StreamType);
            var cui = this.authentication.GetCurrentUserId();
            subscriptionHandler.MarkRead(cui, dto.Id, dto.SubscriptionId);
            //subscriptionHandler.AddEventViewed(cui, dto.Id);
        }

        public void MarkClicked(MarkClickedDto dto)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            this.rssEventsRepository.MarkClicked(dto.Id, currentUserId);
        }

        public void MarkEntriesRead(MarkReadDto dto)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(dto.StreamType);
            //var toberead = RssToMarkRead(dto.DisplayedItems);
            //subscriptionHandler.MarkRead(toberead);
        }

        public void MarkEntriesSkipped(MarkSkippedDto model)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(model.StreamType);
            var cui = this.authentication.GetCurrentUserId();
            var ids = RssToMarkRead(model.Entries);
            subscriptionHandler.MarkSkipped(model.SubscriptionId, ids);

        }

        private static List<long> RssToMarkRead(string model)
        {
            var separator = new[] { ";", "," };
            var rssToMarkRead = model.Split(separator, StringSplitOptions.None).Select(long.Parse).ToList();
            return rssToMarkRead;
        }
    }
}