namespace IsThereAnyNews.Dtos.Feeds
{
    public class FeedsGetEntries
    {
        public long FeedId { get; set; }
        public long Take => 50;
        public long Skip { get; set; }
    }
}