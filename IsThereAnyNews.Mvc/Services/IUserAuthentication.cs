using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Mvc.Services
{
    using System.Security.Claims;

    public interface IUserAuthentication
    {
        string GetCurrentUserSocialLoginId();
        ClaimsPrincipal GetCurrentUser();
        AuthenticationTypeProvider GetCurrentUserLoginProvider();
    }
}