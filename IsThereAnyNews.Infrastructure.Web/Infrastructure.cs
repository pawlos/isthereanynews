using System;
using System.Collections.Generic;
using System.Linq;

namespace IsThereAnyNews.Infrastructure.Web
{
    using System.Security.Claims;
    using System.Web;
    using IsThereAnyNews.SharedData;

    public class Infrastructure: IInfrastructure
    {
        public ClaimsPrincipal GetCurrentUser()
        {
            return HttpContext.Current.GetOwinContext()
                              .Authentication.User;
        }

        public long GetCurrentUserId()
        {
            long.TryParse(
              this.GetCurrentUser()
                  .Claims.SingleOrDefault(claim => claim.Type == ItanClaimTypes.ApplicationIdentifier)
                  ?.Value,
              out var r);
            return r;
        }

        public bool CurrentUserIsAuthenticated()
        {
            return HttpContext.Current.GetOwinContext()
                              .Authentication.User.Identity.IsAuthenticated;
        }

        public AuthenticationTypeProvider GetCurrentUserLoginProvider(ClaimsIdentity identity)
        {
            var issuer = identity.Claims.First(claim => !string.IsNullOrWhiteSpace(claim.Issuer))
                                 .Issuer;
            Enum.TryParse(issuer, true, out AuthenticationTypeProvider enumResult);
            return enumResult;
        }


        public List<ItanRole> GetCurrentUserRoles()
        {
            var roles = this.GetCurrentUser()
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

        public Claim FindUserClaimNameIdentifier(ClaimsIdentity identity)
        {
            var identifier = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            return identifier;
        }
    }
}
