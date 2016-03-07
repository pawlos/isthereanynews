using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using IsThereAnyNews.Mvc.Models;
using IsThereAnyNews.Mvc.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class ApplicationLoginService : ILoginService
    {
        private readonly ApplicationUserStore applicationUserStore;
        private readonly ApplicationUserManager applicationUserManager;
        private readonly IUserAuthentication authentication;

        public ApplicationLoginService() : this(
            new ApplicationUserStore(),
            new ApplicationUserManager(),
            new UserAuthentication())
        {

        }

        private ApplicationLoginService(
            ApplicationUserStore applicationUserStore,
            ApplicationUserManager applicationUserManager,
            IUserAuthentication authentication)
        {
            this.applicationUserStore = applicationUserStore;
            this.applicationUserManager = applicationUserManager;
            this.authentication = authentication;
        }

        public async Task RegisterIfNewUser()
        {
            var currentUserId = this.authentication.GetCurrentUserId();

            var user = await this.applicationUserStore.FindByIdAsync(currentUserId);
            if (user != null)
            {
                return;
            }

            var errorList = await RegisterCurrentUser();
        }

        private async Task<List<string>> RegisterCurrentUser()
        {
            var user = this.authentication.GetCurrentUser();
            var claims = user.Claims.ToList();
            var identifier = claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            var name = claims.First(x => x.Type == ClaimTypes.Name);

            var applicationUser = new ApplicationUser(identifier.Value, name.Value);
            var identityResult = await this.applicationUserManager.CreateAsync(applicationUser);
            return identityResult.Errors.ToList();
        }
    }
}