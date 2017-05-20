namespace IsThereAnyNews.Dtos.Feeds
{
    public class FeedsGetPublic
    {
        public int Take => 50;
        public int Skip { get; set; }
    }
}