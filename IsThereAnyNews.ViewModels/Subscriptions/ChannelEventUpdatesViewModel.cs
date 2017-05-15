using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels.Subscriptions
{
    public class ChannelEventUpdatesViewModel : ISubscriptionViewModel
    {
        public virtual StreamType StreamType => StreamType.ChannelUpdate;
        public string Count { get; set; }
        public long Id { get; }
        public string Name => "Channel updates";
        public string IconType => "fa-refresh";
    }
}