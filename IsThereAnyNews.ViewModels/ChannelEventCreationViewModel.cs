namespace IsThereAnyNews.ViewModels
{
    using IsThereAnyNews.SharedData;

    public class ChannelEventCreationViewModel : ChannelEventViewModel
    {
        public override StreamType StreamType => StreamType.Channel;
    }
}