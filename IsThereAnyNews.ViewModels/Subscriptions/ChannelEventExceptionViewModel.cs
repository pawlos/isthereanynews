namespace IsThereAnyNews.ViewModels.Subscriptions
{
    using IsThereAnyNews.SharedData;

    public class ChannelEventExceptionViewModel: ISubscriptionViewModel
    {
        public virtual StreamType StreamType => StreamType.Exception;
        public string Count { get; set; }
        public long Id { get; }
        public string Name => "Exceptions";
        public string IconType => "fa-exclamation";
    }
}