namespace IsThereAnyNews.ViewModels
{
    using IsThereAnyNews.SharedData;

    public class ChannelEventExceptionViewModel : ChannelEventViewModel
    {
        public override StreamType StreamType => StreamType.Exception;
    }
}