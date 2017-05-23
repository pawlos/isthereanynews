namespace IsThereAnyNews.Dtos.Feeds
{
    public class FeedsGetPublic
    {
        public int Take => 21;
        public int Skip { get; set; }
    }
}