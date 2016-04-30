using System.Collections.Generic;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.Services
{
    public interface IOpmlImporterService
    {
        void AddToCurrentUserChannelList(List<RssChannel> importFromUpload);
        List<RssChannel> ParseToRssChannelList(OpmlImporterIndexDto dto);
        void AddNewChannelsToGlobalSpace(List<RssChannel> channelList);
    }
}