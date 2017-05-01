
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels.RssChannel
{
    public class RssChannelSubscriptionViewModel : ISubscriptionViewModel
    {
        public string Count { get; set; }
        public string Name { get; set; }
        public long Id { get; set; }

        public StreamType StreamType => StreamType.Rss;
    }
}