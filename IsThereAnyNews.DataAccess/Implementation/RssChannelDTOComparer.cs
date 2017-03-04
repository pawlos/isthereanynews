using IsThereAnyNews.ProjectionModels;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelDTOComparer : System.Collections.Generic.IEqualityComparer<RssEntryDTO>
    {
        public bool Equals(RssEntryDTO x, RssEntryDTO y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(RssEntryDTO obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}