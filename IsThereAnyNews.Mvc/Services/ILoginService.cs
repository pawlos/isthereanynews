namespace IsThereAnyNews.Mvc.Services
{
    public interface ILoginService
    {
        void RegisterIfNewUser();
        void AddUserIdToClaims();
    }
}