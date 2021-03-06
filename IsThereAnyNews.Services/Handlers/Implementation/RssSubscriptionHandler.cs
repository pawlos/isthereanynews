namespace IsThereAnyNews.Services.Handlers.Implementation
{
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos.Feeds;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.Services.Handlers.ViewModels;
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

        public ISubscriptionContentIndexViewModel GetSubscriptionViewModel(long userId, FeedsGetRead input)
        {
            var rssEntryToReadDtos = this.entityRepository.LoadRss(input.FeedId, userId, input.Skip, input.Take);
            var rssEntryToReadViewModels = rssEntryToReadDtos.Select(
                x => new RssEntryToReadViewModel
                         {
                             RssEntryViewModel =
                                 new RssEntryViewModel
                                     {
                                         Id = x.Id,
                                         Title = x.Title,
                                         PreviewText =
                                             this.htmlStripper.GetContentOnly(
                                                 x.PreviewText),
                                         PublicationDate = x.PublicationDate,
                                         Url = x.Url,
                                         SubscriptionId = input.FeedId
                                     }
                         }).ToList();
            var rssChannelInformationDto = this.entityRepository.LoadChannelChannelInformation(input.FeedId);
            var viewModel = new RssSubscriptionIndexViewModel(
                input.FeedId,
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