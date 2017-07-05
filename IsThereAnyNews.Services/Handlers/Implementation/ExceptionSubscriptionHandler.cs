namespace IsThereAnyNews.Services.Handlers.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Dtos.Feeds;
    using IsThereAnyNews.Services.Handlers.ViewModels;
    using IsThereAnyNews.ViewModels;

    public class ExceptionSubscriptionHandler: ISubscriptionHandler
    {
        private readonly IEntityRepository entityRepository;

        public ExceptionSubscriptionHandler(IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        public ISubscriptionContentIndexViewModel GetSubscriptionViewModel(long userId, FeedsGetRead input)
        {
            var loadAllRssEntriesForUserAndChannel = this.LoadExceptionEvents(userId, input.Skip, input.Take);
            var subscriptionIndexViewModel = new ExceptionSubscriptionIndexViewModel(
                0,
                "Exceptions",
                DateTime.MinValue,
                loadAllRssEntriesForUserAndChannel);
            var rssSubscriptionIndexViewModel = subscriptionIndexViewModel;
            return rssSubscriptionIndexViewModel;
        }

        public void MarkClicked(long cui, long id, long subscriptionId)
        {
            this.entityRepository.MarkExceptionActivityClicked(cui, id);
        }

        public void MarkNavigated(long userId, long rssId, long dtoSubscriptionId)
        {
        }

        public void MarkSkipped(long cui, long subscriptionId, List<long> entries)
        {
            this.entityRepository.MarkExceptionActivitySkipped(cui, entries);
        }

        private List<RssEntryToReadViewModel> LoadExceptionEvents(long userId, int skip, int take)
        {
            var loadLatestExceptions = this.LoadLatestExceptions(userId, skip, take);
            var rssEntryToReadViewModels = loadLatestExceptions.Select(
                s => new RssEntryToReadViewModel
                {
                    // Id = s.Id,
                    // IsRead = false,
                    RssEntryViewModel =
                                 new RssEntryViewModel
                                 {
                                     Id = s.Id,
                                     PublicationDate = s.Occured,
                                     Url = string.Empty,
                                     Title = s.Typeof,
                                     PreviewText =
                                             $"Message: <br/>{s.Message}<br/> StackTrace:<br/>{s.StackTrace}<br/> Source:<br/>{s.Source}<br/>",
                                     SubscriptionId = 0
                                 }
                });
            return rssEntryToReadViewModels.ToList();
        }

        private List<ExceptionEventDto> LoadLatestExceptions(long userId, int skip, int take)
        {
            var loadLatest = this.entityRepository.LoadExceptionList(userId, skip, take);
            return loadLatest;
        }
    }
}