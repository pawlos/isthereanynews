namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;

    public class TopReadChannelsViewModel
    {
        public TopReadChannelsViewModel(List<ChannelWithSubscriptionCountViewModel> viewModels)
        {
            this.ViewModels = viewModels;
        }

        public List<ChannelWithSubscriptionCountViewModel> ViewModels { get; set; }
    }
}