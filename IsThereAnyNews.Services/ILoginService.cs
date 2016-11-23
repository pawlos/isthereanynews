namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.SharedData;
    using System.Security.Claims;

    public interface ILoginService
    {
        void RegisterIfNewUser(ClaimsIdentity identity);

        void StoreCurrentUserIdInSession(ClaimsIdentity identity);

        void StoreItanRolesToSession(ClaimsIdentity identity);

        void AssignToUserRole(ClaimsIdentity identity);

        bool IsUserRegistered(ClaimsIdentity identity);

        RegistrationSupported GetCurrentRegistrationStatus();

        bool CanRegisterIfWithinLimits();
    }
}