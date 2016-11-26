namespace IsThereAnyNews.Services
{
    using System.Collections.Generic;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels.Mess;

    public interface IOpmlImporterService
    {
        List<RssSourceWithUrlAndTitle> ParseToRssChannelList(OpmlImporterIndexDto dto);

        void AddNewChannelsToGlobalSpace(List<RssSourceWithUrlAndTitle> channelList);

        void Import(OpmlImporterIndexDto dto);
    }
}