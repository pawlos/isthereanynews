namespace IsThereAnyNews.Dtos.Feeds
{
    using IsThereAnyNews.SharedData;

    public class FeedsGetRead
    {
        public FeedsGetRead()
        {
            this.ShowReadEntries = ShowReadEntries.Hide;
        }

        public long FeedId { get; set; }
        public StreamType StreamType { get; set; }
        public ShowReadEntries ShowReadEntries { get; set; }
        public int Skip { get; set; }
        public int Take => 21;
    }
}