namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;

    using IsThereAnyNews.SharedData;

    public class UserAuthentication : IUserAuthentication
    {
        public bool CurrentUserIsAuthenticated()
        {
            return HttpContext.Current.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            return HttpContext.Current.GetOwinContext().Authentication.User;
        }

        public long GetCurrentUserId()
        {
            long r = 0;
            long.TryParse(
                this.GetCurrentUser()
                    .Claims.SingleOrDefault(claim => claim.Type == ItanClaimTypes.ApplicationIdentifier)?.Value,
                out r);
            return r;
        }

        public AuthenticationTypeProvider GetCurrentUserLoginProvider(ClaimsIdentity identity)
        {
            var issuer = identity.Claims.First(claim => !string.IsNullOrWhiteSpace(claim.Issuer)).Issuer;
            AuthenticationTypeProvider enumResult;
            Enum.TryParse(issuer, true, out enumResult);
            return enumResult;
        }

        public List<ItanRole> GetCurrentUserRoles()
        {
            var roles =
                this.GetCurrentUser()
                    .Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => (ItanRole)Enum.Parse(typeof(ItanRole), c.Value))
                    .ToList();
            return roles;
        }

        public string GetUserSocialIdFromIdentity(ClaimsIdentity identity)
        {
            var claim = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            return claim.Value;
        }
    }
}