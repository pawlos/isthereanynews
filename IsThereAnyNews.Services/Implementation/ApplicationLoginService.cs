namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;

    public class ApplicationLoginService : ILoginService
    {
        private readonly IUserAuthentication authentication;
        private readonly IUserRepository userRepository;
        private readonly ISocialLoginRepository socialLoginRepository;
        private readonly ISessionProvider sessionProvider;
        private readonly IUserRoleRepository repositoryUserRoles;
        private readonly IMapper mapper;

        private readonly IApplicationSettingsRepository applicationSettingsRepository;

        public ApplicationLoginService(
            IUserAuthentication authentication,
            IUserRepository userRepository,
            ISocialLoginRepository socialLoginRepository,
            ISessionProvider sessionProvider,
            IUserRoleRepository repositoryUserRoles,
            IMapper mapper,
            IApplicationSettingsRepository applicationSettingsRepository)
        {
            this.authentication = authentication;
            this.userRepository = userRepository;
            this.socialLoginRepository = socialLoginRepository;
            this.sessionProvider = sessionProvider;
            this.repositoryUserRoles = repositoryUserRoles;
            this.mapper = mapper;
            this.applicationSettingsRepository = applicationSettingsRepository;
        }

        public void RegisterIfNewUser()
        {
            var socialLogin = this.FindUserSocialLogin();

            if (socialLogin == null)
            {
                this.RegisterCurrentSocialLogin();
                this.StoreCurrentUserIdInSession();
                this.AssignToUserRole();
            }
        }

        public SocialLogin FindUserSocialLogin()
        {
            var authenticationTypeProvider = this.authentication.GetCurrentUserLoginProvider();
            var currentUserId = this.authentication.GetCurrentUserSocialLoginId();
            var socialLogin = this.socialLoginRepository.FindSocialLogin(currentUserId, authenticationTypeProvider);
            return socialLogin;
        }

        public void AssignToUserRole()
        {
            var cui = this.sessionProvider.GetCurrentUserId();
            this.repositoryUserRoles.AssignUserRole(cui);
        }

        public bool IsUserRegistered()
        {
            return this.FindUserSocialLogin() != null;
        }

        public RegistrationSupported GetCurrentRegistrationStatus()
        {
            return this.applicationSettingsRepository.GetCurrentRegistrationStatus();
        }

        public bool CanRegisterIfWithinLimits()
        {
            return this.applicationSettingsRepository.CanRegisterWithinLimits();
        }

        public void StoreCurrentUserIdInSession()
        {
            var currentUserSocialLoginId = this.authentication.GetCurrentUserSocialLoginId();
            var currentUserLoginProvider = this.authentication.GetCurrentUserLoginProvider();

            var findSocialLogin = this.socialLoginRepository.FindSocialLogin(currentUserSocialLoginId, currentUserLoginProvider);
            this.sessionProvider.SetUserId(findSocialLogin.UserId);
        }

        public void StoreItanRolesToSession()
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            var itanRoles = this.repositoryUserRoles.GetRolesForUser(currentUserId);
            var claims = this.mapper.Map<List<Claim>>(itanRoles);
            this.sessionProvider.SaveClaims(claims);
        }

        private void RegisterCurrentSocialLogin()
        {
            var identifier = this.FindCurrentUserClaimNameIdentifier();
            var newUser = this.CreateNewApplicationUser();
            var authenticationTypeProvider = this.GetUserAuthenticationProviderFromAuthentication();
            this.CreateAndAssignNewSocialLoginForApplicationUser(identifier, authenticationTypeProvider, newUser);
        }

        private AuthenticationTypeProvider GetUserAuthenticationProviderFromAuthentication()
        {
            return this.authentication.GetCurrentUserLoginProvider();
        }

        private User CreateNewApplicationUser()
        {
            var name = this.FindCurrentUserClaimName();
            var email = this.FindCurrentUserClaimEmail();
            return this.userRepository.CreateNewUser(name.Value, email.Value);
        }

        private void CreateAndAssignNewSocialLoginForApplicationUser(Claim identifier,
                                                                     AuthenticationTypeProvider authenticationTypeProvider,
                                                                     User newUser)
        {
            var socialLogin = new SocialLogin(identifier.Value, authenticationTypeProvider, newUser.Id);
            this.socialLoginRepository.SaveToDatabase(socialLogin);
        }

        private Claim FindCurrentUserClaimNameIdentifier()
        {
            var user = this.authentication.GetCurrentUser();
            var claims = user.Claims.ToList();
            var identifier = claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            return identifier;
        }

        private Claim FindCurrentUserClaimName()
        {
            var user = this.authentication.GetCurrentUser();
            var claims = user.Claims.ToList();
            var identifier = claims.First(x => x.Type == ClaimTypes.Name);
            return identifier;
        }

        private Claim FindCurrentUserClaimEmail()
        {
            var user = this.authentication.GetCurrentUser();
            var claims = user.Claims.ToList();
            var identifier = claims.First(x => x.Type == ClaimTypes.Email);
            return identifier;
        }

    }
}