namespace IsThereAnyNews.Services
{
    using System.Security.Claims;

    using IsThereAnyNews.SharedData;

    public interface IUserAuthentication
    {
        string GetCurrentUserSocialLoginId();
        ClaimsPrincipal GetCurrentUser();
        AuthenticationTypeProvider GetCurrentUserLoginProvider();
        bool CurrentUserIsAuthenticated();
    }
}