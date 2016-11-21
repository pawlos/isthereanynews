namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public interface IUpdateService
    {
        void UpdateGlobalRss();
        void UpdateChannel(RssChannel id);
    }
}