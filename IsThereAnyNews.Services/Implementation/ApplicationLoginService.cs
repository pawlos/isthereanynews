namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.SharedData;

    public class ApplicationLoginService : ILoginService
    {
        private readonly IApplicationSettingsRepository applicationSettingsRepository;
        private readonly IUserAuthentication authentication;
        private readonly IMapper mapper;
        private readonly IUserRoleRepository repositoryUserRoles;
        private readonly ISocialLoginRepository socialLoginRepository;
        private readonly IUserRepository userRepository;
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

        public void AssignToUserRole(ClaimsIdentity identity)
        {
            var cui = long.Parse(identity.Claims.Single(x => x.Type == ItanClaimTypes.ApplicationIdentifier).Value);
            this.repositoryUserRoles.AssignUserRole(cui);
        }

        public bool CanRegisterIfWithinLimits()
        {
            return this.applicationSettingsRepository.CanRegisterWithinLimits();
        }

        public RegistrationSupported GetCurrentRegistrationStatus()
        {
            return this.applicationSettingsRepository.GetCurrentRegistrationStatus();
        }

        public bool IsUserRegistered(ClaimsIdentity identity)
        {
            var authenticationTypeProvider = this.authentication.GetCurrentUserLoginProvider(identity);
            var userId = this.authentication.GetUserSocialIdFromIdentity(identity);
            var userIsRegistered = this.socialLoginRepository.UserIsRegistered(authenticationTypeProvider, userId);
            return userIsRegistered;
        }

        public void RegisterIfNewUser(ClaimsIdentity identity)
        {
            var isUserRegistered = this.IsUserRegistered(identity);

            if (isUserRegistered)
            {
                this.RegisterCurrentSocialLogin(identity);
                this.StoreCurrentUserIdInSession(identity);
                this.AssignToUserRole(identity);
            }
        }
        public void StoreCurrentUserIdInSession(ClaimsIdentity identity)
        {
            if (identity.Claims.Any(x => x.Type == ItanClaimTypes.ApplicationIdentifier))
            {
                return;
            }

            var currentUserSocialLoginId = this.authentication.GetUserSocialIdFromIdentity(identity);
            var currentUserLoginProvider = this.authentication.GetCurrentUserLoginProvider(identity);

            var userId = this.socialLoginRepository.GetUserId(currentUserSocialLoginId, currentUserLoginProvider);
            identity.AddClaim(new Claim(ItanClaimTypes.ApplicationIdentifier, userId.ToString(), ClaimValueTypes.Integer64, "ITAN"));
        }

        public void StoreItanRolesToSession(ClaimsIdentity identity)
        {
            var currentUserId = long.Parse(identity.Claims.Single(x => x.Type == ItanClaimTypes.ApplicationIdentifier).Value);
            var itanRoles = this.repositoryUserRoles.GetRolesTypesForUser(currentUserId);
            var claims = this.mapper.Map<List<Claim>>(itanRoles);
            identity.AddClaims(claims);
        }

        private void CreateAndAssignNewSocialLoginForApplicationUser(Claim identifier,
                                                                     AuthenticationTypeProvider authenticationTypeProvider,
                                                                     long newUserId)
        {
            this.socialLoginRepository.CreateNewSociaLogin(identifier.Value, authenticationTypeProvider, newUserId);
        }

        private long CreateNewApplicationUser(ClaimsIdentity identity)
        {
            var name = identity.Claims.Single(x => x.Type == ClaimTypes.Name);
            var email = identity.Claims.Single(x => x.Type == ClaimTypes.Email);
            return this.userRepository.CreateNewUser(name.Value, email.Value);
        }

        private Claim FindUserClaimNameIdentifier(ClaimsIdentity identity)
        {
            var identifier = identity.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            return identifier;
        }

        private AuthenticationTypeProvider GetUserAuthenticationProviderFromAuthentication(ClaimsIdentity identity)
        {
            return this.authentication.GetCurrentUserLoginProvider(identity);
        }

        private void RegisterCurrentSocialLogin(ClaimsIdentity identity)
        {
            var identifier = this.FindUserClaimNameIdentifier(identity);
            var newUserId = this.CreateNewApplicationUser(identity);
            var authenticationTypeProvider = this.GetUserAuthenticationProviderFromAuthentication(identity);
            this.CreateAndAssignNewSocialLoginForApplicationUser(identifier, authenticationTypeProvider, newUserId);
        }
    }
}