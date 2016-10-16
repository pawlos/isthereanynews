namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;

    public interface IAdminRepository
    {
        ApplicationConfiguration LoadApplicationConfiguration();

        long GetNumberOfRegisteredUsers();

        long GetNumberOfRssSubscriptions();

        long GetNumberOfRssNews();

        void ChangeApplicationRegistration(RegistrationSupported dtoStatus);

        void ChangeUserLimit(long dtoLimit);
    }
}