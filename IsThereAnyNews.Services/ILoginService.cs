namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;

    public interface ILoginService
    {
        void RegisterIfNewUser();
        void StoreCurrentUserIdInSession();
        void StoreItanRolesToSession();
        void AssignToUserRole();

        bool IsUserRegistered();

        RegistrationSupported GetCurrentRegistrationStatus();

        bool CanRegisterIfWithinLimits();
    }
}