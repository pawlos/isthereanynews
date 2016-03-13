using System.Linq;
using System.Security.Claims;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public class ApplicationLoginService : ILoginService
    {
        private readonly IUserAuthentication authentication;
        private readonly IUserRepository userRepository;
        private readonly ISocialLoginRepository socialLoginRepository;

        public ApplicationLoginService() : this(
            new UserAuthentication(),
            new UserRepository(),
            new SocialLoginRepository())
        {
        }

        private ApplicationLoginService(
            IUserAuthentication authentication,
            IUserRepository userRepository,
            ISocialLoginRepository socialLoginRepository)
        {
            this.authentication = authentication;
            this.userRepository = userRepository;
            this.socialLoginRepository = socialLoginRepository;
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

        public void AddUserIdToClaims()
        {
            var currentUserSocialLoginId = this.authentication.GetCurrentUserSocialLoginId();
            var currentUserLoginProvider = this.authentication.GetCurrentUserLoginProvider();

            var findSocialLogin = this.socialLoginRepository.FindSocialLogin(currentUserSocialLoginId, currentUserLoginProvider);
            var claimsPrincipal = this.authentication.GetCurrentUser();

            var claim = new Claim(ClaimTypes.UserData, findSocialLogin.UserId.ToString());
            claimsPrincipal.AddIdentity(new ClaimsIdentity(new[] { claim }));
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