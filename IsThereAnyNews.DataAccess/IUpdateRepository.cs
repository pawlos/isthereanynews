namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.ProjectionModels;

    public interface IUpdateRepository
    {
        List<RssChannelForUpdateDTO> LoadAllGlobalRssChannels();
    }
}