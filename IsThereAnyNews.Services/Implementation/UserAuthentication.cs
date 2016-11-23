namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.SharedData;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;

    public class UserAuthentication : IUserAuthentication
    {
        public string GetUserSocialIdFromIdentity(ClaimsIdentity identity)
        {
            var claim = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            return claim.Value;
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            return HttpContext.Current.GetOwinContext().Authentication.User;
        }

        public AuthenticationTypeProvider GetCurrentUserLoginProvider(ClaimsIdentity identity)
        {
            var issuer = identity.Claims.First(claim => !string.IsNullOrWhiteSpace(claim.Issuer)).Issuer;
            AuthenticationTypeProvider enumResult;
            Enum.TryParse(issuer, true, out enumResult);
            return enumResult;
        }

        public bool CurrentUserIsAuthenticated()
        {
            return HttpContext
                .Current
                .GetOwinContext()
                .Authentication
                .User
                .Identity
                .IsAuthenticated;
        }

        public long GetCurrentUserId()
        {
            long r = 0;
            long.TryParse(
                this.GetCurrentUser()
                    .Claims
                    .SingleOrDefault(claim => claim.Type == ItanClaimTypes.ApplicationIdentifier)
                    ?.Value, out r);
            return r;
        }
    }
}