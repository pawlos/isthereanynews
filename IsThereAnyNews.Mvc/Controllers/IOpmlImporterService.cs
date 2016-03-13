using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Collections.Generic;
    using Dtos;

    public interface IOpmlImporterService
    {
        void AddToCurrentUserChannelList(List<RssChannel> importFromUpload);
        List<RssChannel> ParseToRssChannelList(OpmlImporterIndexDto dto);
        void AddNewChannelsToGlobalSpace(List<RssChannel> channelList);
    }
}