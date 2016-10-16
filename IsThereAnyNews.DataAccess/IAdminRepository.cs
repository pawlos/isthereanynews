namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public interface IAdminRepository
    {
        ApplicationConfiguration LoadApplicationConfiguration();

        long GetNumberOfRegisteredUsers();

        long GetNumberOfRssSubscriptions();

        long GetNumberOfRssNews();
    }
}