using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.ViewModels
{
    public class RssChannelSubscriptionViewModel
    {
        public RssChannelSubscriptionViewModel(RssChannelSubscription subscription)
        {
            this.Id = subscription.Id;
            this.Title = subscription.Title;
        }

        public string ChannelUrl { get; set; }
        public string Title { get; set; }
        public long Id { get; set; }
    }
}