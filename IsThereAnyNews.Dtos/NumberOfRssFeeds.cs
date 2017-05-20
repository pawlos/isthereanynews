namespace IsThereAnyNews.Dtos
{
    public class NumberOfRssFeeds
    {
        public int Count { get; }

        public NumberOfRssFeeds(int count)
        {
            this.Count = count;
        }
    }
}