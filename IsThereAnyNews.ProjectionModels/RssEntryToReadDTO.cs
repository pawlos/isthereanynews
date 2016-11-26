namespace IsThereAnyNews.ProjectionModels
{
    public class RssEntryToReadDTO
    {
        public bool IsRead { get; set; }
        public long Id { get; set; }
        public RssEntryDTO RssEntryDto { get; set; }
    }
}