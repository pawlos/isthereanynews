using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Dtos
{
    public class MarkReadDto
    {
        public StreamType StreamType { get; set; }
        public long Id { get; set; }
        public string DisplayedItems { get; set; }
    }
}