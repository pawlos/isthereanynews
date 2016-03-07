namespace IsThereAnyNews.Mvc.Services
{
    using Microsoft.AspNet.Identity;
    using Models;

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager() : this(new ApplicationUserStore())
        {
        }

        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }
    }
}