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

        private List<RssEntryToReadViewModel> LoadUpdateEvents(long userId, int skip, int take)
        {
            var channelUpdateEventDtos = this.entityRepository.LoadUpdateEvents(userId, skip, take);
            var x = channelUpdateEventDtos.Select(s => new RssEntryToReadViewModel
            {
                RssEntryViewModel =
                new RssEntryViewModel
                {
                    Id = s.Id,
                    PublicationDate = s.Updated,
                    Url = string.Empty,
                    Title = s.ChannelTitle,
                    SubscriptionId = 0
                }
            });
            return x.ToList();
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
    }
}