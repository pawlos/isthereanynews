using System.Web;

namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public class SessionProvider : ISessionProvider
    {
        private const string UserIdKey = "SessionProvider.Long.UserId";
        public void SetUserId(long userId)
        {
            HttpContext.Current.Session[UserIdKey] = userId;
        }

        public long GetCurrentUserId()
        {
            return (long)HttpContext.Current.Session[UserIdKey];
        }
    }
}