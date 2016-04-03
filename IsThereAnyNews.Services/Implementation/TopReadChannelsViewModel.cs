using System.Collections.Generic;

namespace IsThereAnyNews.Services.Implementation
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