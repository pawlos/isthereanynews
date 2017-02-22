using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels
{
    public class ChannelEventViewModel
    {
        public string Count { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public StreamType StreamType => StreamType.Channel;
    }
}