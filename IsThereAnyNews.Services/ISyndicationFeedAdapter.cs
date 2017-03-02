namespace IsThereAnyNews.Services
{
    using System.Collections.Generic;

    using IsThereAnyNews.ProjectionModels.Mess;

    public interface ISyndicationFeedAdapter
    {
        List<SyndicationItemAdapter> Load(string url);
    }
}