using System.Collections.Generic;

namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.SharedData;
    using System.Security.Claims;

    public interface IUserAuthentication
    {
        string GetUserSocialIdFromIdentity(ClaimsIdentity identity);

        ClaimsPrincipal GetCurrentUser();

        AuthenticationTypeProvider GetCurrentUserLoginProvider(ClaimsIdentity identity);

        bool CurrentUserIsAuthenticated();

        long GetCurrentUserId();
        List<ItanRole> GetCurrentUserRoles();
    }
}