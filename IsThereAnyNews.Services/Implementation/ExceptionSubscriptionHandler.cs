namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class ExceptionSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IEntityRepository entityRepository;

        public ExceptionSubscriptionHandler(IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        public void AddEventSkipped(long cui, string entries)
        {
            throw new System.NotImplementedException();
        }

        public void AddEventViewed(long dtoId)
        {
            throw new System.NotImplementedException();
        }

        public void AddEventViewed(long cui, long id)
        {
            throw new System.NotImplementedException();
        }

        public RssSubscriptionIndexViewModel GetSubscriptionViewModel(long subscriptionId, ShowReadEntries showReadEntries)
        {
            var channelInformationViewModel = new ChannelInformationViewModel
            {
                Created = DateTime.MinValue,
                Title = "Application exceptions"
            };

            var loadAllRssEntriesForUserAndChannel = this.LoadExceptionEvents(showReadEntries);

            var subscriptionIndexViewModel = new RssSubscriptionIndexViewModel(0, channelInformationViewModel,
                loadAllRssEntriesForUserAndChannel,
                StreamType.Channel);

            var rssSubscriptionIndexViewModel = subscriptionIndexViewModel;

            return rssSubscriptionIndexViewModel;
        }

        public void MarkRead(List<long> displayedItems)
        {
            throw new System.NotImplementedException();
        }

        public void MarkRead(long userId, long rssId, long dtoSubscriptionId)
        {
            throw new System.NotImplementedException();
        }

        public void MarkSkipped(long modelSubscriptionId, List<long> ids)
        {
            throw new System.NotImplementedException();
        }

        private List<RssEntryToReadViewModel> LoadExceptionEvents(ShowReadEntries showReadEntries)
        {
            var loadLatestExceptions = this.LoadLatestExceptions(100);
            var rssEntryToReadViewModels = loadLatestExceptions.Select(s => new RssEntryToReadViewModel
            {
                Id = s.Id,
                IsRead = false,
                RssEntryViewModel = new RssEntryViewModel
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

        private List<Dtos.ExceptionEventDto> LoadLatestExceptions(int eventCount)
        {
            var loadLatest = this.entityRepository.LoadLatest(eventCount);
            return loadLatest;
        }
    }
}