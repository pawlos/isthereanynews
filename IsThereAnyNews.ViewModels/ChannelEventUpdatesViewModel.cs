namespace IsThereAnyNews.ViewModels
{
    using IsThereAnyNews.SharedData;

    public class ChannelEventUpdatesViewModel : ChannelEventViewModel
    {
        public override StreamType StreamType => StreamType.Channel;
    }
}