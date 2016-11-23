namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.Services.Implementation;
    using System.Collections.Generic;

    public interface ISyndicationFeedAdapter
    {
        List<SyndicationItemAdapter> Load(string url);
    }
}