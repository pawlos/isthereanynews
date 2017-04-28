namespace IsThereAnyNews.Infrastructure.Web
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using IsThereAnyNews.SharedData;

    public interface IInfrastructure
    {
        ClaimsPrincipal GetCurrentUser();
        long GetCurrentUserId();
        bool CurrentUserIsAuthenticated();
        AuthenticationTypeProvider GetCurrentUserLoginProvider(ClaimsIdentity identity);
        List<ItanRole> GetCurrentUserRoles();
        string GetUserSocialIdFromIdentity(ClaimsIdentity identity);
        Claim FindUserClaimNameIdentifier(ClaimsIdentity identity);
    }
}