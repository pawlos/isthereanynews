namespace IsThereAnyNews.Services.Handlers.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class ChannelUpdatesSubscriptionHandler: ISubscriptionHandler
    {
        private readonly IEntityRepository entityRepository;

        public ChannelUpdatesSubscriptionHandler(IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        public ISubscriptionContentIndexViewModel GetSubscriptionViewModel(long userId, long subscriptionId, ShowReadEntries showReadEntries)
        {
            var loadAllRssEntriesForUserAndChannel = this.LoadUpdateEvents();

            var subscriptionIndexViewModel = new ChannelUpdateSubscriptionIndexViewModel(0,
                "Channel update events",
                DateTime.MinValue,
                loadAllRssEntriesForUserAndChannel);

            return subscriptionIndexViewModel;
        }

        public void MarkClicked(long cui, long id, long subscriptionId) => throw new NotImplementedException();
        public void MarkNavigated(long userId, long rssId, long dtoSubscriptionId) => throw new NotImplementedException();
        public void MarkSkipped(long cui, long subscriptionId, List<long> entries) => throw new NotImplementedException();

        private List<RssEntryToReadViewModel> LoadUpdateEvents()
        {
            var dtos = this.entityRepository.LoadUpdateEvents();
            var rssEntryToReadViewModels =
                    dtos.Select(d =>
                                    new RssEntryToReadViewModel
                                    {
                                        Id = d.Id,
                                        IsRead = false,
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