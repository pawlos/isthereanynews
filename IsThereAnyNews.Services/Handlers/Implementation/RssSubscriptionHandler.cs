namespace IsThereAnyNews.Services.Handlers.Implementation
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
        private readonly IUserAuthentication authentication;

        private readonly IEntityRepository entityRepository;

        private readonly IHtmlStripper htmlStripper;

        private readonly IMapper mapper;

        public RssSubscriptionHandler(
            IMapper mapper,
            IUserAuthentication authentication,
            IHtmlStripper htmlStripper,
            IEntityRepository entityRepository)
        {
            this.mapper = mapper;
            this.authentication = authentication;
            this.htmlStripper = htmlStripper;
            this.entityRepository = entityRepository;
        }

        public void AddEventSkipped(long cui, string entries)
        {
            var x = entries.Split(new[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
            var ids = x.Select(l => long.Parse(l)).ToList();
            this.entityRepository.AddEventRssSkipped(cui, ids);
        }

        public void AddEventViewed(long dtoId)
        {
            var cui = this.authentication.GetCurrentUserId();
            this.entityRepository.AddEventRssViewed(cui, dtoId);
        }

        public void AddEventViewed(long cui, long id)
        {
            this.entityRepository.AddEventRssViewed(cui, id);
        }

        public RssSubscriptionIndexViewModel GetSubscriptionViewModel(
            long subscriptionId,
            ShowReadEntries showReadEntries)
        {
            return this.GetRssSubscriptionIndexViewModel(subscriptionId, showReadEntries);
        }

        public void MarkRead(List<long> ids)
        {
            this.entityRepository.MarkChannelRead(ids);
        }

        public void MarkRead(long userId, long rssId, long dtoSubscriptionId)
        {
            this.entityRepository.InsertReadRssToRead(userId, rssId, dtoSubscriptionId);
        }

        public void MarkSkipped(long modelSubscriptionId, List<long> ids)
        {
            this.entityRepository.MarkRssEntriesSkipped(modelSubscriptionId, ids);
        }

        private RssSubscriptionIndexViewModel GetRssSubscriptionIndexViewModel(
            long subscriptionId,
            ShowReadEntries showReadEntries)
        {
            var rssEntryToReadDtos = this.entityRepository.LoadRss(subscriptionId, showReadEntries);
            var rssEntryToReadViewModels =
                rssEntryToReadDtos.OrderByDescending(o => o.PublicationDate)
                    .Select(
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
                                                PreviewText =
                                                    this.htmlStripper
                                                        .GetContentOnly(
                                                            x.PreviewText),
                                                PublicationDate =
                                                    x.PublicationDate,
                                                Url = x.Url,
                                                SubscriptionId =
                                                    subscriptionId
                                            }
                                })
                    .ToList();

            var rssChannelInformationDto = this.entityRepository.LoadChannelChannelInformation(subscriptionId);

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

        private RssSubscriptionIndexViewModel GetRssSubscriptionIndexViewModelOld(
            long subscriptionId,
            ShowReadEntries showReadEntries)
        {
            var currentUserId = this.authentication.GetCurrentUserId();

            if (!this.entityRepository.DoesUserOwnsUserSubscription(subscriptionId, currentUserId))
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
                                                         ? this.entityRepository.LoadAllUnreadEntriesFromSubscription(
                                                             subscriptionId)
                                                         : this.entityRepository.LoadAllEntriesFromSubscription(
                                                             subscriptionId);

            var channelInformation = this.entityRepository.LoadChannelChannelInformation(subscriptionId);
            var channelInformationViewModel = new ChannelInformationViewModel
                                                  {
                                                      Title = channelInformation.Title,
                                                      Created = channelInformation.Created
                                                  };

            var rssEntryToReadViewModels =
                this.mapper.Map<List<RssEntryToReadDTO>, List<RssEntryToReadViewModel>>(
                    loadAllRssEntriesForUserAndChannel);

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