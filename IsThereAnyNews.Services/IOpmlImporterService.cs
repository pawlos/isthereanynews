namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using System.Collections.Generic;

    public interface IOpmlImporterService
    {
        void AddToCurrentUserChannelList(List<RssChannel> importFromUpload);

        List<RssChannel> ParseToRssChannelList(OpmlImporterIndexDto dto);

        void AddNewChannelsToGlobalSpace(List<RssChannel> channelList);

        void Import(OpmlImporterIndexDto dto);
    }
}