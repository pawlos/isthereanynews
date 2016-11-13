
namespace IsThereAnyNews.ViewModels
{
    using IsThereAnyNews.SharedData;

    public class RssChannelSubscriptionViewModel
    {
        public int Count { get; set; }
        public string ChannelUrl { get; set; }
        public string Title { get; set; }
        public long Id { get; set; }

        public StreamType StreamType => StreamType.Rss;
    }
}