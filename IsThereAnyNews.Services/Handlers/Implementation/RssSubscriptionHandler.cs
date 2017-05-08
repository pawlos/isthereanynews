namespace IsThereAnyNews.Services.Handlers.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class RssSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IEntityRepository entityRepository;

        private readonly IHtmlStripper htmlStripper;

        public RssSubscriptionHandler(IHtmlStripper htmlStripper, IEntityRepository entityRepository)
        {
            this.htmlStripper = htmlStripper;
            this.entityRepository = entityRepository;
        }

        public ISubscriptionContentIndexViewModel GetSubscriptionViewModel(long userId, long subscriptionId, ShowReadEntries showReadEntries)
        {
            var rssEntryToReadDtos = this.entityRepository.LoadRss(subscriptionId, userId);
            var rssEntryToReadViewModels = rssEntryToReadDtos.OrderByDescending(o => o.PublicationDate)
                .Select(x =>
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
                    })
                .ToList();
            var rssChannelInformationDto = this.entityRepository.LoadChannelChannelInformation(subscriptionId);
            var viewModel = new RssSubscriptionIndexViewModel(subscriptionId,
                rssChannelInformationDto.Title,
                rssChannelInformationDto.Created,
                rssEntryToReadViewModels);
            return viewModel;
        }

        public void MarkClicked(long cui, long id, long subscriptionId)
        {
            this.entityRepository.MarkRssClicked(id, subscriptionId);
            this.entityRepository.AddEventRssClicked(cui, id);
        }

        public void MarkNavigated(long userId, long rssId, long subscriptionId)
        {
            this.entityRepository.MarkRssNavigated(rssId, subscriptionId);
            this.entityRepository.AddEventRssNavigated(userId, rssId);
        }

        public void MarkSkipped(long cui, long subscriptionId, List<long> entries)
        {
            this.entityRepository.MarkRssEntriesSkipped(subscriptionId, entries);
            this.entityRepository.AddEventRssSkipped(cui, entries);
        }
    }
}