namespace IsThereAnyNews.EntityFramework.Models
{
    public sealed class RssChannel
    {
        public RssChannel(string url, string title)
        {
            Id = 0;
            Url = url;
            Title = title;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}