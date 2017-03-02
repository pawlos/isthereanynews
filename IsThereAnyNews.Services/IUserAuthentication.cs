namespace IsThereAnyNews.Services
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using IsThereAnyNews.SharedData;

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