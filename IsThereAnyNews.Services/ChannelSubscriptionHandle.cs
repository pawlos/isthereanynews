using System;
using System.Collections.Generic;
using IsThereAnyNews.SharedData;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public class ChannelSubscriptionHandle: ISubscriptionHandler
    {
        public RssSubscriptionIndexViewModel GetSubscriptionViewModel(long subscriptionId, ShowReadEntries showReadEntries)
        {
            var channelInformationViewModel = new ChannelInformationViewModel
            {
                Created = DateTime.MinValue,
                Title = "Channel update events"
            };
            var rssSubscriptionIndexViewModel = 
            new RssSubscriptionIndexViewModel(0, channelInformationViewModel, new List<RssEntryToReadViewModel>(),
                StreamType.Channel);
            return rssSubscriptionIndexViewModel;
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