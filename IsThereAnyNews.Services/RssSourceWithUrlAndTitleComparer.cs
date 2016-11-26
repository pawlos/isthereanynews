namespace IsThereAnyNews.Services
{
    using System.Collections.Generic;

    using IsThereAnyNews.ProjectionModels.Mess;

    public class RssSourceWithUrlAndTitleComparer : IEqualityComparer<RssSourceWithUrlAndTitle>
    {
        public bool Equals(RssSourceWithUrlAndTitle x, RssSourceWithUrlAndTitle y)
        {
            return x.Url == y.Url;
        }

        public int GetHashCode(RssSourceWithUrlAndTitle obj)
        {
            return obj.Url.GetHashCode();
        }
    }
}