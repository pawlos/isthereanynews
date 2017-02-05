using System.Data.Entity.Infrastructure;

namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class RssSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionsRepository;

        private readonly IRssEntriesToReadRepository rssToReadRepository;
        private readonly IMapper mapper;

        private readonly IRssEventRepository rssEventsRepository;

        private readonly IUserAuthentication authentication;

        public RssSubscriptionHandler(
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository,
            IRssEntriesToReadRepository rssToReadRepository,
            IMapper mapper,
            IRssEventRepository rssEventsRepository,
            IUserAuthentication authentication)
        {
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.rssToReadRepository = rssToReadRepository;
            this.mapper = mapper;
            this.rssEventsRepository = rssEventsRepository;
            this.authentication = authentication;
        }

        public RssSubscriptionIndexViewModel GetSubscriptionViewModel(long subscriptionId, ShowReadEntries showReadEntries)
        {
            return this.GetRssSubscriptionIndexViewModel(subscriptionId, showReadEntries);
        }

        private RssSubscriptionIndexViewModel GetRssSubscriptionIndexViewModel(
            long subscriptionId,
            ShowReadEntries showReadEntries)
        {
            var rssEntryToReadDtos = this.rssToReadRepository.LoadRss(subscriptionId, showReadEntries);
            var rssEntryToReadViewModels = rssEntryToReadDtos.Select(
                x =>
                    new RssEntryToReadViewModel
                        {
                            Id = 0,
                            IsRead = false,
                            RssEntryViewModel =
                                new RssEntryViewModel
                                    {
                                        Id = x.Id,
                                        Title = x.Title,
                                        PreviewText = x.PreviewText,
                                        PublicationDate = x.PublicationDate,
                                        Url = x.Url,
                                        SubscriptionId = subscriptionId
                                    }
                        }).ToList();

            var viewModel = new RssSubscriptionIndexViewModel(
                                subscriptionId,
                                new ChannelInformationViewModel { Created = DateTime.MinValue, Title = "FIX ME ASAP" },
                                rssEntryToReadViewModels,
                                StreamType.Rss);
            viewModel.SubscriptionId = subscriptionId;
            return viewModel;

        }

        public void MarkRead(List<long> ids)
        {
            this.rssSubscriptionsRepository.MarkRead(ids);
        }

        public void AddEventViewed(long dtoId)
        {
            var cui = this.authentication.GetCurrentUserId();
            this.rssEventsRepository.AddEventRssViewed(cui, dtoId);
        }

        public void MarkSkipped(long modelSubscriptionId, List<long> ids)
        {
            this.rssToReadRepository.MarkEntriesSkipped(modelSubscriptionId, ids);
        }

        public void MarkRead(long userId, long rssId, long dtoSubscriptionId)
        {
            this.rssToReadRepository.InsertReadRssToRead(userId, rssId, dtoSubscriptionId);
        }

        private RssSubscriptionIndexViewModel GetRssSubscriptionIndexViewModelOld(long subscriptionId, ShowReadEntries showReadEntries)
        {
            var currentUserId = this.authentication.GetCurrentUserId();

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

            var rssEntryToReadViewModels = this.mapper.Map<List<RssEntryToReadDTO>, List<RssEntryToReadViewModel>>(loadAllRssEntriesForUserAndChannel);

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