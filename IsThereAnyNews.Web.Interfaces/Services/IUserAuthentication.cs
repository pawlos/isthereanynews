namespace IsThereAnyNews.Web.Interfaces.Services
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using IsThereAnyNews.SharedData;

    public interface IUserAuthentication
    {
        bool CurrentUserIsAuthenticated();

        ClaimsPrincipal GetCurrentUser();

        long GetCurrentUserId();

        AuthenticationTypeProvider GetCurrentUserLoginProvider(ClaimsIdentity identity);

        List<ItanRole> GetCurrentUserRoles();

        string GetUserSocialIdFromIdentity(ClaimsIdentity identity);
    }
}