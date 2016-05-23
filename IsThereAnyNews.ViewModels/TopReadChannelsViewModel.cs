using System.Collections.Generic;

namespace IsThereAnyNews.ViewModels
{
    public class TopReadChannelsViewModel
    {
        public List<ChannelWithSubscriptionCountViewModel> ViewModels { get; set; }

        public TopReadChannelsViewModel(List<ChannelWithSubscriptionCountViewModel> viewModels)
        {
            ViewModels = viewModels;
        }
    }
}