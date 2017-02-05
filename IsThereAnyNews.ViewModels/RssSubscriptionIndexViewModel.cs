namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.SharedData;

    public class RssSubscriptionIndexViewModel
    {
        public RssSubscriptionIndexViewModel(
            long subscriptionId,
            ChannelInformationViewModel channelInformation, 
            List<RssEntryToReadViewModel> loadAllRssEntriesForUserAndChannel, 
            StreamType streamType)
        {
            this.SubscriptionId = subscriptionId;
            this.ChannelInformation = channelInformation;
            this.StreamType = streamType;
            this.RssEntryToReadViewModels = loadAllRssEntriesForUserAndChannel;
        }


        public ChannelInformationViewModel ChannelInformation { get; set; }
        public StreamType StreamType { get; set; }
        public List<RssEntryToReadViewModel> RssEntryToReadViewModels { get; set; }
        public long SubscriptionId { get; set; }
        public string DisplayedRss => string.Join(";", RssEntryToReadViewModels.Select(x => x.RssEntryViewModel.Id));
    }
}