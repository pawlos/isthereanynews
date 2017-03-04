namespace IsThereAnyNews.ViewModels
{
    using IsThereAnyNews.SharedData;

    public class ChannelEventViewModel
    {
        public string Count { get; set; }

        public long Id { get; set; }

        public string Name { get; set; }

        public virtual StreamType StreamType => StreamType.Channel;
    }
}