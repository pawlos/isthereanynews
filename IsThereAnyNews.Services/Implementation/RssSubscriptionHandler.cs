using System.Data.Entity.Infrastructure;

namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.HtmlStrip;
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
        private readonly IHtmlStripper htmlStripper;
        private readonly IRssChannelsSubscriptionsRepository channelsSubscriptionsRepository;

        public RssSubscriptionHandler(
            IRssChannelsSubscriptionsRepository rssSubscriptionsRepository,
            IRssEntriesToReadRepository rssToReadRepository,
            IMapper mapper,
            IRssEventRepository rssEventsRepository,
            IUserAuthentication authentication,
            IHtmlStripper htmlStripper,
            IRssChannelsSubscriptionsRepository channelsSubscriptionsRepository)
        {
            this.rssSubscriptionsRepository = rssSubscriptionsRepository;
            this.rssToReadRepository = rssToReadRepository;
            this.mapper = mapper;
            this.rssEventsRepository = rssEventsRepository;
            this.authentication = authentication;
            this.htmlStripper = htmlStripper;
            this.channelsSubscriptionsRepository = channelsSubscriptionsRepository;
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
            var rssEntryToReadViewModels = rssEntryToReadDtos.OrderByDescending(o => o.PublicationDate).Select(
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
                                    PreviewText = this.htmlStripper.GetContentOnly(x.PreviewText),
                                    PublicationDate = x.PublicationDate,
                                    Url = x.Url,
                                    SubscriptionId = subscriptionId
                                }
                    }).ToList();

            var rssChannelInformationDto = this.channelsSubscriptionsRepository.LoadChannelInformation(subscriptionId);

            var viewModel = new RssSubscriptionIndexViewModel(
                                subscriptionId,
                                new ChannelInformationViewModel
                                {
                                    Created = rssChannelInformationDto.Created,
                                    Title = rssChannelInformationDto.Title
                                },
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

        public void AddEventViewed(long cui, long id)
        {
            this.rssEventsRepository.AddEventRssViewed(cui, id);
        }

        public void AddEventSkipped(long cui, string entries)
        {
            var x = entries.Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
            var ids = x.Select(l => long.Parse(l)).ToList();
            this.rssEventsRepository.AddEventRssSkipped(cui, ids);
        }
    }
}