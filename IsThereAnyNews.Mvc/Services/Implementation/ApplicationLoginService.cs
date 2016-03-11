using System.Linq;
using System.Security.Claims;
using IsThereAnyNews.Mvc.Models;
using IsThereAnyNews.Mvc.Repositories;

namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public class ApplicationLoginService : ILoginService
    {
        private readonly IUserAuthentication authentication;
        private readonly IApplicationUserRepository applicationUserRepository;

        public ApplicationLoginService() : this(
            new UserAuthentication(), new ApplicationUserRepository())
        {
        }

        private ApplicationLoginService(
            IUserAuthentication authentication,
            IApplicationUserRepository applicationUserRepository)
        {
            this.authentication = authentication;
            this.applicationUserRepository = applicationUserRepository;
        }

        public void RegisterIfNewUser()
        {
            var currentUserId = this.authentication.GetCurrentUserId();

            var user = this.applicationUserRepository.FindById(currentUserId);
            if (user == null)
            {
                this.RegisterCurrentUser();
            }
        }

        private void RegisterCurrentUser()
        {
            var user = this.authentication.GetCurrentUser();
            var claims = user.Claims.ToList();
            var identifier = claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            var name = claims.First(x => x.Type == ClaimTypes.Name);

            var applicationUser = new ItanUser(identifier.Value, name.Value);
            this.applicationUserRepository.SaveNewUserToDatabase(applicationUser);
        }
    }
}