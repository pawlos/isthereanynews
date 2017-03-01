namespace IsThereAnyNews.Services.Implementation
{
    public class ChannelEventExceptionViewModel : ViewModels.ChannelEventViewModel
    {
        public override SharedData.StreamType StreamType => SharedData.StreamType.Exception;
    }
}