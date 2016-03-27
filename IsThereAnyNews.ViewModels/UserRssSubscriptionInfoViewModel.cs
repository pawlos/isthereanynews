namespace IsThereAnyNews.ViewModels
{
    public class UserRssSubscriptionInfoViewModel
    {
        public long ChannelSubscriptionId { get; set; }
        public bool IsSubscribed => ChannelSubscriptionId > 0;

        public UserRssSubscriptionInfoViewModel(long channelSubscriptionId)
        {
            ChannelSubscriptionId = channelSubscriptionId;
        }

    }
}