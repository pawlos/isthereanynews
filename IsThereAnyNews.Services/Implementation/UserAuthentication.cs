using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Services.Implementation
{
    public class UserAuthentication : IUserAuthentication
    {
        public string GetCurrentUserSocialLoginId()
        {
            var claimsPrincipal = GetCurrentUserClaimsFromOwinAuthentication();
            var claim = claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            return claim.Value;
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            return GetCurrentUserClaimsFromOwinAuthentication();
        }

        public AuthenticationTypeProvider GetCurrentUserLoginProvider()
        {
            var claimsPrincipal = GetCurrentUserClaimsFromOwinAuthentication();
            var issuer = claimsPrincipal.Claims.First(claim => !string.IsNullOrWhiteSpace(claim.Issuer)).Issuer;
            AuthenticationTypeProvider enumResult;
            Enum.TryParse(issuer, true, out enumResult);
            return enumResult;
        }

        private static ClaimsPrincipal GetCurrentUserClaimsFromOwinAuthentication()
        {
            return HttpContext.Current.GetOwinContext().Authentication.User;
        }
    }
}