using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public class UserAuthentication : IUserAuthentication
    {
        public string GetCurrentUserId()
        {
            var userId = GetOwinContext().Authentication.User.Identity.GetUserId();
            return userId;
        }

        private static IOwinContext GetOwinContext()
        {
            return HttpContext.Current.GetOwinContext();
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            return GetOwinContext().Authentication.User;
        }
    }
}