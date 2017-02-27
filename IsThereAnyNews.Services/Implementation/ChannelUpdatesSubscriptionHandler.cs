using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.SharedData;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services.Implementation
{
    public class ChannelUpdatesSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IChannelEventsRepository channelEventsRepository;

        public ChannelUpdatesSubscriptionHandler(IChannelEventsRepository channelEventsRepository)
        {
            this.channelEventsRepository = channelEventsRepository;
        }

        public RssSubscriptionIndexViewModel GetSubscriptionViewModel(long subscriptionId,
            ShowReadEntries showReadEntries)
        {
            var channelInformationViewModel = new ChannelInformationViewModel
            {
                Created = DateTime.MinValue,
                Title = "Channel update events"
            };

            var loadAllRssEntriesForUserAndChannel = this.LoadUpdateEvents(showReadEntries);

            var subscriptionIndexViewModel = new RssSubscriptionIndexViewModel(0, channelInformationViewModel,
                loadAllRssEntriesForUserAndChannel,
                StreamType.Channel);

            var rssSubscriptionIndexViewModel = subscriptionIndexViewModel;

            return rssSubscriptionIndexViewModel;
        }

        private List<RssEntryToReadViewModel> LoadUpdateEvents(ShowReadEntries showReadEntries)
        {
            var dtos = this.channelEventsRepository.LoadUpdateEvents(100);
            var rssEntryToReadViewModels = dtos.Select(d => new RssEntryToReadViewModel
            {
                Id = d.Id,
                IsRead = false,
                RssEntryViewModel = new RssEntryViewModel
                {
                    Id = d.Id,
                    Title = d.ChannelTitle,
                    PublicationDate = d.Updated,
                    Url = string.Empty
                }
            });
            return rssEntryToReadViewModels.ToList();
        }

        public void MarkRead(List<long> displayedItems)
        {
            throw new System.NotImplementedException();
        }

        public void AddEventViewed(long dtoId)
        {
            throw new System.NotImplementedException();
        }

        public void MarkSkipped(long modelSubscriptionId, List<long> ids)
        {
            throw new System.NotImplementedException();
        }

        public void MarkRead(long userId, long rssId, long dtoSubscriptionId)
        {
            throw new System.NotImplementedException();
        }

        public void AddEventViewed(long cui, long id)
        {
            throw new System.NotImplementedException();
        }

        public void AddEventSkipped(long cui, string entries)
        {
            throw new System.NotImplementedException();
        }
    }
}