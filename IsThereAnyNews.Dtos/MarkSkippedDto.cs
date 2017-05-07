using System.Collections.Generic;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Dtos
{
    public class EntriesSkippedDto
    {
        public List<long> Entries { get; set; }
        public StreamType StreamType { get; set; }
        public long SubscriptionId { get; set; }
    }
}