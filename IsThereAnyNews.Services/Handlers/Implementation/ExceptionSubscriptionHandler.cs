namespace IsThereAnyNews.Services.Handlers.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class ExceptionSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IEntityRepository entityRepository;

        public ExceptionSubscriptionHandler(IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        public ISubscriptionContentIndexViewModel GetSubscriptionViewModel(long userId, long subscriptionId, ShowReadEntries showReadEntries)
        {
            var loadAllRssEntriesForUserAndChannel = this.LoadExceptionEvents(showReadEntries);
            var subscriptionIndexViewModel = new ExceptionSubscriptionIndexViewModel(0,
                "Exceptions",
                DateTime.MinValue,
                loadAllRssEntriesForUserAndChannel);
            var rssSubscriptionIndexViewModel = subscriptionIndexViewModel;
            return rssSubscriptionIndexViewModel;
        }

        public void MarkClicked(long cui, long id, long subscriptionId) => throw new NotImplementedException();
        public void MarkNavigated(long userId, long rssId, long dtoSubscriptionId) => throw new NotImplementedException();
        public void MarkSkipped(long cui, long subscriptionId, List<long> entries) => throw new NotImplementedException();

        private List<RssEntryToReadViewModel> LoadExceptionEvents(ShowReadEntries showReadEntries)
        {
            var loadLatestExceptions = this.LoadLatestExceptions(100);
            var rssEntryToReadViewModels =
                    loadLatestExceptions.Select(
                                                s =>
                                                    new RssEntryToReadViewModel
                                                    {
                                                        Id = s.Id,
                                                        IsRead = false,
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

        private List<ExceptionEventDto> LoadLatestExceptions(int eventCount)
        {
            var loadLatest = this.entityRepository.LoadLatest(eventCount);
            return loadLatest;
        }
    }
}