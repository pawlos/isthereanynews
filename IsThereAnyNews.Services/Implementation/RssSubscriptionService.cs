using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
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
        private readonly IRssChannelsRepository rssChannelsRepository;
        private readonly IMapper mapper;

        public RssSubscriptionService(
            ISessionProvider sessionProvider,
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository,
            IRssEntriesToReadRepository rssToReadRepository,
            IRssEventRepository rssEventsRepository,
            IUsersSubscriptionRepository usersSubscriptionRepository,
            IRssChannelsRepository rssChannelsRepository, 
            IMapper mapper)
        {
            this.sessionProvider = sessionProvider;
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.rssToReadRepository = rssToReadRepository;
            this.rssEventsRepository = rssEventsRepository;
            this.usersSubscriptionRepository = usersSubscriptionRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.mapper = mapper;
        }


        public RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(StreamType streamType, long subscriptionId, ShowReadEntries showReadEntries)
        {
            switch (streamType)
            {
                case StreamType.Rss:
                    return GetRssSubscriptionIndexViewModel(subscriptionId, showReadEntries);
                case StreamType.Person:
                    return GetPersonSubscriptionIndexViewModel(subscriptionId, showReadEntries);
                default:
                    throw new ArgumentOutOfRangeException(nameof(streamType), streamType, null);
            }
        }

        private RssSubscriptionIndexViewModel GetPersonSubscriptionIndexViewModel(long subscriptionId, ShowReadEntries showReadEntries)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();

            if (!this.usersSubscriptionRepository.DoesUserOwnsSubscription(subscriptionId, currentUserId))
            {
                var ci = new ChannelInformationViewModel
                {
                    Title = "You are not subscribed to this user",
                    Created = DateTime.MaxValue
                };

                var rssSubscriptionIndexViewModel = 
                    new RssSubscriptionIndexViewModel(subscriptionId, 
                    ci, 
                    new List<RssEntryToReadViewModel>(),
                    StreamType.Person);
                return rssSubscriptionIndexViewModel;
            }

            List<UserSubscriptionEntryToRead> loadAllUnreadEntriesFromSubscription = null;
            if (showReadEntries != ShowReadEntries.Show)
            {
                loadAllUnreadEntriesFromSubscription = this.usersSubscriptionRepository
                    .LoadAllUnreadEntriesFromSubscription(subscriptionId);
            }
            else
            {
                loadAllUnreadEntriesFromSubscription = this.usersSubscriptionRepository
                    .LoadAllEntriesFromSubscription(subscriptionId);
            }

            var channelInformation =
                this.usersSubscriptionRepository
                    .LoadChannelInformation(subscriptionId);

            var channelInformationViewModel = new ChannelInformationViewModel
            {
                Title = channelInformation.Title,
                Created = channelInformation.Created
            };

            var rssEntryToReadViewModels = this.mapper.Map<List<RssEntryToReadViewModel>>(loadAllUnreadEntriesFromSubscription);

            var viewModel = new RssSubscriptionIndexViewModel(
                subscriptionId,
                channelInformationViewModel,
                rssEntryToReadViewModels,
                StreamType.Person);

            return viewModel;
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

        public void MarkRead(MarkReadDto dto)
        {
            switch (dto.StreamType)
            {
                case StreamType.Rss:
                    MarkRssItemsRead(dto.DisplayedItems);
                    break;
                case StreamType.Person:
                    MarkPersonItemsRead(dto.DisplayedItems);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        private void MarkPersonItemsRead(string displayedItems)
        {
            var ids =
                displayedItems.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => long.Parse(i))
                    .ToList();
            this.rssEventsRepository.MarkRead(ids);
        }

        private void MarkRssItemsRead(string displayedItems)
        {
            var ids = displayedItems.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => long.Parse(i))
                .ToList();
            this.rssSubscriptionsRepository.MarkRead(ids);
        }
    }
}