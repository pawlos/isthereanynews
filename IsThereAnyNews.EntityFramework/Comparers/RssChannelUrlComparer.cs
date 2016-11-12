namespace IsThereAnyNews.EntityFramework.Comparers
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class RssChannelUrlComparer : IEqualityComparer<RssChannel>
    {
        public bool Equals(RssChannel x, RssChannel y)
        {
            return x.Url == y.Url;
        }

        public int GetHashCode(RssChannel obj)
        {
            return obj.Url.GetHashCode();
        }
    }
}