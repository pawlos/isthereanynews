using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Dtos
{
    public class MarkSkippedDto
    {
        public string Entries { get; set; }
        public StreamType StreamType { get; set; }
        public long SubscriptionId { get; set; }
    }
}