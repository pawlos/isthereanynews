namespace IsThereAnyNews.Services.Implementation
{
    using AutoMapper;
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    public class ApplicationLoginService : ILoginService
    {
        private readonly IUserAuthentication authentication;
        private readonly IUserRepository userRepository;
        private readonly ISocialLoginRepository socialLoginRepository;

        private readonly IUserRoleRepository repositoryUserRoles;
        private readonly IMapper mapper;

        private readonly IApplicationSettingsRepository applicationSettingsRepository;

        public ApplicationLoginService(
            IUserAuthentication authentication,
            IUserRepository userRepository,
            ISocialLoginRepository socialLoginRepository,
            IUserRoleRepository repositoryUserRoles,
            IMapper mapper,
            IApplicationSettingsRepository applicationSettingsRepository)
        {
            this.authentication = authentication;
            this.userRepository = userRepository;
            this.socialLoginRepository = socialLoginRepository;
            this.repositoryUserRoles = repositoryUserRoles;
            this.mapper = mapper;
            this.applicationSettingsRepository = applicationSettingsRepository;
        }

        public void RegisterIfNewUser(ClaimsIdentity identity)
        {
            var socialLogin = this.FindUserSocialLogin(identity);

            if (socialLogin == null)
            {
                this.RegisterCurrentSocialLogin(identity);
                this.StoreCurrentUserIdInSession(identity);
                this.AssignToUserRole(identity);
            }
        }

        public SocialLogin FindUserSocialLogin(ClaimsIdentity identity)
        {
            var authenticationTypeProvider = this.authentication.GetCurrentUserLoginProvider(identity);
            var userId = this.authentication.GetUserSocialIdFromIdentity(identity);
            var socialLogin = this.socialLoginRepository.FindSocialLogin(userId, authenticationTypeProvider);
            return socialLogin;
        }

        public void AssignToUserRole(ClaimsIdentity identity)
        {
            var cui = long.Parse(identity.Claims.Single(x => x.Type == ItanClaimTypes.ApplicationIdentifier).Value);
            this.repositoryUserRoles.AssignUserRole(cui);
        }

        public bool IsUserRegistered(ClaimsIdentity identity)
        {
            return this.FindUserSocialLogin(identity) != null;
        }

        public RegistrationSupported GetCurrentRegistrationStatus()
        {
            return this.applicationSettingsRepository.GetCurrentRegistrationStatus();
        }

        public bool CanRegisterIfWithinLimits()
        {
            return this.applicationSettingsRepository.CanRegisterWithinLimits();
        }

        public void StoreCurrentUserIdInSession(ClaimsIdentity identity)
        {
            if (identity.Claims.Any(x => x.Type == ItanClaimTypes.ApplicationIdentifier))
            {
                return;
            }

            var currentUserSocialLoginId = this.authentication.GetUserSocialIdFromIdentity(identity);
            var currentUserLoginProvider = this.authentication.GetCurrentUserLoginProvider(identity);

            var findSocialLogin = this.socialLoginRepository.FindSocialLogin(currentUserSocialLoginId, currentUserLoginProvider);
            identity.AddClaim(new Claim(ItanClaimTypes.ApplicationIdentifier, findSocialLogin.UserId.ToString(), ClaimValueTypes.Integer64, "ITAN"));
        }

        public void StoreItanRolesToSession(ClaimsIdentity identity)
        {
            var currentUserId = long.Parse(identity.Claims.Single(x => x.Type == ItanClaimTypes.ApplicationIdentifier).Value);
            var itanRoles = this.repositoryUserRoles.GetRolesForUser(currentUserId);
            var claims = this.mapper.Map<List<Claim>>(itanRoles);
            identity.AddClaims(claims);
        }

        private void RegisterCurrentSocialLogin(ClaimsIdentity identity)
        {
            var identifier = this.FindUserClaimNameIdentifier(identity);
            var newUser = this.CreateNewApplicationUser(identity);
            var authenticationTypeProvider = this.GetUserAuthenticationProviderFromAuthentication(identity);
            this.CreateAndAssignNewSocialLoginForApplicationUser(identifier, authenticationTypeProvider, newUser);
        }

        private AuthenticationTypeProvider GetUserAuthenticationProviderFromAuthentication(ClaimsIdentity identity)
        {
            return this.authentication.GetCurrentUserLoginProvider(identity);
        }

        private User CreateNewApplicationUser(ClaimsIdentity identity)
        {
            var name = identity.Claims.Single(x => x.Type == ClaimTypes.Name);
            var email = identity.Claims.Single(x => x.Type == ClaimTypes.Email);
            return this.userRepository.CreateNewUser(name.Value, email.Value);
        }

        private void CreateAndAssignNewSocialLoginForApplicationUser(Claim identifier,
                                                                     AuthenticationTypeProvider authenticationTypeProvider,
                                                                     User newUser)
        {
            var socialLogin = new SocialLogin(identifier.Value, authenticationTypeProvider, newUser.Id);
            this.socialLoginRepository.SaveToDatabase(socialLogin);
        }

        private Claim FindUserClaimNameIdentifier(ClaimsIdentity identity)
        {
            var identifier = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
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