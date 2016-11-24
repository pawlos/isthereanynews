namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.SharedData;

    public interface IAdminRepository
    {
        ApplicationConfigurationDTO LoadApplicationConfiguration();

        long GetNumberOfRegisteredUsers();

        long GetNumberOfRssSubscriptions();

        long GetNumberOfRssNews();

        void ChangeApplicationRegistration(RegistrationSupported dtoStatus);

        void ChangeUserLimit(long dtoLimit);
    }
}