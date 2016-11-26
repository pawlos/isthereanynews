namespace IsThereAnyNews.ProjectionModels
{
    public class RssChannelSubscriptionDTO
    {
        public int Count { get; set; }
        public string ChannelUrl { get; set; }
        public string Title { get; set; }
        public long Id { get; set; }
        public long RssChannelId { get; set; }
    }
}