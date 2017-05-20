namespace IsThereAnyNews.Services.Handlers.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Services.Handlers.ViewModels;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class ChannelCreationsSubscriptionHandler: ISubscriptionHandler
    {
        private readonly IEntityRepository entityRepository;

        public ChannelCreationsSubscriptionHandler(IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        public ISubscriptionContentIndexViewModel GetSubscriptionViewModel(long userId, long subscriptionId, ShowReadEntries showReadEntries)
        {
            var loadAllRssEntriesForUserAndChannel = this.LoadCreationEvents(userId);

            var subscriptionIndexViewModel = new ChannelCreationSubscriptionIndexViewModel(0,
                "Channel creation events",
                DateTime.MinValue,
                loadAllRssEntriesForUserAndChannel);

            return subscriptionIndexViewModel;
        }

        private List<RssEntryToReadViewModel> LoadCreationEvents(long userId)
        {
            var dtos = this.entityRepository.LoadCreationEvents(userId).OrderBy(o=>o.Updated);
            var rssEntryToReadViewModels =
                    dtos.Select(d =>
                                    new RssEntryToReadViewModel
                                    {
                                        //Id = d.Id,
                                        //IsRead = false,
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

        public void MarkNavigated(long userId, long rssId, long dtoSubscriptionId)
        {
        }

        public void MarkClicked(long cui, long id, long subscriptionId)
        {
            this.entityRepository.MarkChannelCreateClicked(cui, id);
        }

        public void MarkSkipped(long cui, long subscriptionId, List<long> entries)
        {
            this.entityRepository.MarkChannelCreateSkipped(cui, entries);
        }
    }
}