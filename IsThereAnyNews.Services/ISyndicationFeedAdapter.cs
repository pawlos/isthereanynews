namespace IsThereAnyNews.Services
{
    using System.Collections.Generic;

    using IsThereAnyNews.Services.Implementation;

    public interface ISyndicationFeedAdapter
    {
        List<SyndicationItemAdapter> Load(string url);
    }
}