namespace IsThereAnyNews.Dtos
{
    public class MarkReadForSubscriptionDto
    {
        public long SubscriptionId { get; set; }
        public string RssEntries { get; set; }
    }
}