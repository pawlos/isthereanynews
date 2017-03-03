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
        private readonly ISubscriptionHandlerFactory subscriptionHandlerFactory;
        private readonly IUserAuthentication authentication;
        private readonly IEntityRepository entityRepository;

        public RssSubscriptionService(
            ISubscriptionHandlerFactory subscriptionHandlerFactory,
            IUserAuthentication authentication, IEntityRepository entityRepository)
        {
            this.subscriptionHandlerFactory = subscriptionHandlerFactory;
            this.authentication = authentication;
            this.entityRepository = entityRepository;
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
            this.entityRepository.MarkAllReadForUserAndSubscription(dto.SubscriptionId, rssToMarkRead);
        }

        public void UnsubscribeCurrentUserFromChannelId(long id)
        {
            var userId = this.authentication.GetCurrentUserId();
            this.entityRepository.DeleteSubscriptionFromUser(id, userId);
        }

        public void SubscribeCurrentUserToChannel(long channelId)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            var isUserSubscribedToChannelUrl = this.entityRepository.IsUserSubscribedToChannelId(currentUserId, channelId);

            if (!isUserSubscribedToChannelUrl)
            {
                this.entityRepository.Subscribe(channelId, currentUserId);
            }
        }

        public void SubscribeCurrentUserToChannel(AddChannelDto channelId)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            var isUserSubscribedToChannelUrl = this.entityRepository.IsUserSubscribedToChannelUrl(currentUserId, channelId.RssChannelLink);

            if (!isUserSubscribedToChannelUrl)
            {
                var idByChannelUrl = this.entityRepository
                    .GetIdByChannelUrl(new List<string> { channelId.RssChannelLink })
                    .Single();

                this.entityRepository.Subscribe(idByChannelUrl, currentUserId, channelId.RssChannelName);
            }
        }

        public void MarkRead(MarkReadDto dto)
        {
            var subscriptionHandler = this.subscriptionHandlerFactory.GetProvider(dto.StreamType);
            var cui = this.authentication.GetCurrentUserId();
            subscriptionHandler.MarkRead(cui, dto.Id, dto.SubscriptionId);
            subscriptionHandler.AddEventViewed(cui, dto.Id);
        }

        public void MarkClicked(MarkClickedDto dto)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            this.entityRepository.MarkClicked(dto.Id, currentUserId);
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
            subscriptionHandler.AddEventSkipped(cui, model.Entries);
        }

        private static List<long> RssToMarkRead(string model)
        {
            var separator = new[] { ";", "," };
            var rssToMarkRead = model.Split(separator, StringSplitOptions.None).Select(long.Parse).ToList();
            return rssToMarkRead;
        }
    }
}