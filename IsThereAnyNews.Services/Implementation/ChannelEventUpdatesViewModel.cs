namespace IsThereAnyNews.Services.Implementation
{
    public class ChannelEventUpdatesViewModel : ViewModels.ChannelEventViewModel
    {
        public override SharedData.StreamType StreamType => SharedData.StreamType.Channel;
    }
}