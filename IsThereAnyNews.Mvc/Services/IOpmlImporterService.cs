using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.Mvc.Dtos;

namespace IsThereAnyNews.Mvc.Services
{
    public interface IOpmlImporterService
    {
        void AddToCurrentUserChannelList(List<RssChannel> importFromUpload);
        List<RssChannel> ParseToRssChannelList(OpmlImporterIndexDto dto);
        void AddNewChannelsToGlobalSpace(List<RssChannel> channelList);
    }
}