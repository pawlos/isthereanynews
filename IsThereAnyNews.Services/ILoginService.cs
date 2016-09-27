namespace IsThereAnyNews.Services
{
    public interface ILoginService
    {
        void RegisterIfNewUser();
        void StoreCurrentUserIdInSession();
        void StoreItanRolesToSession();
        void AssignToUserRole();
    }
}