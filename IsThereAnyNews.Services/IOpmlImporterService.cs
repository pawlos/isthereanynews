namespace IsThereAnyNews.Services
{
    using System.Collections.Generic;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public interface IOpmlImporterService
    {
        void AddToCurrentUserChannelList(List<RssChannel> importFromUpload);
        List<RssChannel> ParseToRssChannelList(OpmlImporterIndexDto dto);
        void AddNewChannelsToGlobalSpace(List<RssChannel> channelList);

        void Import(OpmlImporterIndexDto dto);
    }
}