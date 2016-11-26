namespace IsThereAnyNews.ProjectionModels
{
    public class UserSubscriptionEntryToReadDTO
    {
        public bool IsRead { get; set; }
        public long Id { get; set; }
        public RssEntryDTO RssEntryDto { get; set; }
    }
}