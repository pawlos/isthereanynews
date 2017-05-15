using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels.Subscriptions
{
    public class ChannelEventCreationViewModel: ISubscriptionViewModel
    {
        public virtual StreamType StreamType => StreamType.ChannelUpdate;
        public string Count { get; set; }
        public long Id { get; }
        public string Name => "Channels creation";
        public string IconType => "fa-plus-square";
    }
}