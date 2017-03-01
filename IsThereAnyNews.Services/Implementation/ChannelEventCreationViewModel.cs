namespace IsThereAnyNews.Services.Implementation
{
    public class ChannelEventCreationViewModel : ViewModels.ChannelEventViewModel
    {
        public override SharedData.StreamType StreamType => SharedData.StreamType.Channel;
    }
}