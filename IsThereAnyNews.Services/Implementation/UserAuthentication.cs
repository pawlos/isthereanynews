namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;

    using IsThereAnyNews.SharedData;

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
            return
                long.Parse(
                    this.GetCurrentUser()
                        .Claims.Single(claim => claim.Type == ItanClaimTypes.ApplicationIdentifier)
                        .Value);
        }
    }
}