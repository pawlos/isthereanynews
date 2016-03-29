using System.Collections.Generic;

namespace IsThereAnyNews.ViewModels
{
    public class RssChannelsIndexViewModel
    {
        public List<RssChannelWithStatisticsViewModel> AllChannels { get; set; }

        public RssChannelsIndexViewModel(List<RssChannelWithStatisticsViewModel> allChannels)
        {
            AllChannels = allChannels;
        }
    }
}