namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Collections.Generic;
    using Dtos;
    using Models;

    public interface IOpmlImporterService
    {
        void AddToCurrentUserChannelList(List<RssChannel> importFromUpload);
        List<RssChannel> ImportFromUpload(OpmlImporterIndexDto dto);
    }
}