using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels.Subscriptions
{
    public class ChannelEventCreationViewModel: ISubscriptionViewModel
    {
        public virtual StreamType StreamType => StreamType.ChannelCreation;
        public int Count { get; set; }
        public long Id { get; }
        public string Name => "Channels creation";
        public string IconType => "fa-plus-square";
    }
}