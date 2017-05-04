namespace IsThereAnyNews.Dtos
{
    public class NameAndCountUserSubscription
    {
        public long SubscriptionId { get; set; }
        public long UserId { get; set; }
        public string DisplayName { get; set; }
        public int Count { get; set; }
    }
}