using System.Linq;
using System.Security.Claims;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.Services.Implementation
{
    public class ApplicationLoginService : ILoginService
    {
        private readonly IUserAuthentication authentication;
        private readonly IUserRepository userRepository;
        private readonly ISocialLoginRepository socialLoginRepository;
        private readonly ISessionProvider sessionProvider;

        public ApplicationLoginService(
            IUserAuthentication authentication,
            IUserRepository userRepository,
            ISocialLoginRepository socialLoginRepository,
            ISessionProvider sessionProvider)
        {
            this.authentication = authentication;
            this.userRepository = userRepository;
            this.socialLoginRepository = socialLoginRepository;
            this.sessionProvider = sessionProvider;
        }

        public void RegisterIfNewUser()
        {
            var authenticationTypeProvider = this.authentication.GetCurrentUserLoginProvider();
            var currentUserId = this.authentication.GetCurrentUserSocialLoginId();

            var socialLogin = this.socialLoginRepository.FindSocialLogin(currentUserId, authenticationTypeProvider);
            if (socialLogin == null)
            {
                this.RegisterCurrentSocialLogin();
            }
        }

        public void StoreCurrentUserIdInSession()
        {
            var currentUserSocialLoginId = this.authentication.GetCurrentUserSocialLoginId();
            var currentUserLoginProvider = this.authentication.GetCurrentUserLoginProvider();

            var findSocialLogin = this.socialLoginRepository.FindSocialLogin(currentUserSocialLoginId, currentUserLoginProvider);
            this.sessionProvider.SetUserId(findSocialLogin.UserId);
        }

        private void RegisterCurrentSocialLogin()
        {
            var user = this.authentication.GetCurrentUser();
            var claims = user.Claims.ToList();
            var identifier = claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            var authenticationTypeProvider = this.authentication.GetCurrentUserLoginProvider();

            var newUser = this.userRepository.CreateNewUser();
            var socialLogin = new SocialLogin(identifier.Value, authenticationTypeProvider, newUser.Id);
            this.socialLoginRepository.SaveToDatabase(socialLogin);
        }
    }
}