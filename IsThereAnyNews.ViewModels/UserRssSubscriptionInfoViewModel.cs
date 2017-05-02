
namespace IsThereAnyNews.ViewModels
{
    public class UserRssSubscriptionInfoViewModel
    {
        public long ChannelSubscriptionId { get; set; }

        public bool IsSubscribed => this.ChannelSubscriptionId > 0;
    }
}