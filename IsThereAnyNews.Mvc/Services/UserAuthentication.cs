namespace IsThereAnyNews.Mvc.Services
{
    using System.Security.Claims;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;

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