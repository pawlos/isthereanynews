using System.Security.Claims;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Services
{
    public interface IUserAuthentication
    {
        string GetCurrentUserSocialLoginId();
        ClaimsPrincipal GetCurrentUser();
        AuthenticationTypeProvider GetCurrentUserLoginProvider();
    }
}