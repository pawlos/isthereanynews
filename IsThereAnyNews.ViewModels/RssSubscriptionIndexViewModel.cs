using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels
{
    public class RssSubscriptionIndexViewModel
    {
        public RssSubscriptionIndexViewModel(
            ChannelInformationViewModel channelInformation,
            List<RssEntryToRead> loadAllRssEntriesForUserAndChannel,
            StreamType streamType)
        {
            this.ChannelInformation = channelInformation;
            StreamType = streamType;
            this.RssEntryToReadViewModels = loadAllRssEntriesForUserAndChannel
                .Select(r => new RssEntryToReadViewModel(r)).ToList();
        }

        public RssSubscriptionIndexViewModel(
            ChannelInformationViewModel channelInformationViewModel, 
            List<UserSubscriptionEntryToRead> loadAllRssEntriesForUserAndChannel, 
            StreamType streamType)
        {
            this.ChannelInformation = channelInformationViewModel;
            StreamType = streamType;
            this.RssEntryToReadViewModels =
                loadAllRssEntriesForUserAndChannel.Select(r => new RssEntryToReadViewModel(r)).ToList();
        }

        public ChannelInformationViewModel ChannelInformation { get; set; }
        public StreamType StreamType { get; set; }
        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; set; }
        public long SubscriptionId { get; set; }
        public string DisplayedRss => string.Join(";", RssEntryToReadViewModels.Select(x => x.Id));
    }
}