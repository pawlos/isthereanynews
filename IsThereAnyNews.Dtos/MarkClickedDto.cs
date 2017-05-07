using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Dtos
{
    public class EntryClickedDto
    {
        public StreamType StreamType { get; set; }
        public long Id { get; set; }
        public long SubscriptionId { get; set; }
    }
}