namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;

    public class SessionProvider : ISessionProvider
    {
        private const string UserIdKey = "SessionProvider.Long.UserId";
        private const string UserClaims = "SessionProvider.List.Claims";
        public void SetUserId(long userId)
        {
            HttpContext.Current.Session[UserIdKey] = userId;
        }

        public long GetCurrentUserId()
        {
            object id = HttpContext.Current.Session[UserIdKey] ?? 0L;
            return (long)id;
        }

        public void SaveClaims(IEnumerable<Claim> claims)
        {
            HttpContext.Current.Session[UserClaims] = claims.ToList();
        }

        public List<Claim> LoadClaims()
        {
            return HttpContext.Current.Session[UserClaims] as List<Claim>;
        }
    }
}