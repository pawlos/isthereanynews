namespace IsThereAnyNews.Services.Handlers.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos.Feeds;
    using IsThereAnyNews.Services.Handlers.ViewModels;
    using IsThereAnyNews.ViewModels;

    public class ChannelUpdatesSubscriptionHandler: ISubscriptionHandler
    {
        private readonly IEntityRepository entityRepository;

        public ChannelUpdatesSubscriptionHandler(IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        public ISubscriptionContentIndexViewModel GetSubscriptionViewModel(long userId, FeedsGetRead input)
        {
            var loadAllRssEntriesForUserAndChannel = this.LoadUpdateEvents(userId, input.Skip, input.Take);

            var subscriptionIndexViewModel = new ChannelUpdateSubscriptionIndexViewModel(
                0,
                "Channel update events",
                DateTime.MinValue,
                loadAllRssEntriesForUserAndChannel);

            return subscriptionIndexViewModel;
        }

        private List<RssEntryToReadViewModel> LoadUpdateEvents(long userId, int inputSkip, int inputTake)
        {
            throw new NotImplementedException();
        }

        public void MarkClicked(long cui, long id, long subscriptionId)
        {
            this.entityRepository.MarkChannelUpdateClicked(cui, id);
        }

        public void MarkNavigated(long userId, long rssId, long dtoSubscriptionId)
        {
        }

        public void MarkSkipped(long cui, long subscriptionId, List<long> entries)
        {
            this.entityRepository.MarkChannelUpdateSkipped(cui, entries);
        }

        private List<RssEntryToReadViewModel> LoadUpdateEvents(long userId)
        {
            var dtos = this.entityRepository.LoadUpdateEvents(userId).OrderBy(o => o.Updated);
            var rssEntryToReadViewModels = dtos.Select(
                d => new RssEntryToReadViewModel
                {
                    // Id = d.Id,
                    // IsRead = false,
                    RssEntryViewModel =
                                 new RssEntryViewModel
                                 {
                                     Id = d.Id,
                                     Title = d.ChannelTitle,
                                     PublicationDate = d.Updated,
                                     Url = string.Empty
                                 }
                });
            return rssEntryToReadViewModels.ToList();
        }
    }
}