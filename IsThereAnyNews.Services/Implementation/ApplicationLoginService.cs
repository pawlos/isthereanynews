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
        private readonly IUserAuthentication authentication;
        private readonly IMapper mapper;

        private readonly IEntityRepository entityRepository;

        public ApplicationLoginService(
            IUserAuthentication authentication,
            IMapper mapper,
            IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
            this.authentication = authentication;
            this.mapper = mapper;
        }

        public void AssignToUserRole(ClaimsIdentity identity)
        {
            var cui = long.Parse(identity.Claims.Single(x => x.Type == ItanClaimTypes.ApplicationIdentifier).Value);
            this.entityRepository.AssignUserRole(cui);
        }

        public bool CanRegisterIfWithinLimits()
        {
            return this.entityRepository.CanRegisterWithinLimits();
        }

        public RegistrationSupported GetCurrentRegistrationStatus()
        {
            return this.entityRepository.GetCurrentRegistrationStatus();
        }

        public bool IsUserRegistered(ClaimsIdentity identity)
        {
            var authenticationTypeProvider = this.authentication.GetCurrentUserLoginProvider(identity);
            var userId = this.authentication.GetUserSocialIdFromIdentity(identity);
            var userIsRegistered = this.entityRepository.UserIsRegistered(authenticationTypeProvider, userId);
            return userIsRegistered;
        }

        public void RegisterIfNewUser(ClaimsIdentity identity)
        {
            var isUserRegistered = this.IsUserRegistered(identity);

            if (!isUserRegistered)
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

            var userId = this.entityRepository.GetUserId(currentUserSocialLoginId, currentUserLoginProvider);
            identity.AddClaim(new Claim(ItanClaimTypes.ApplicationIdentifier, userId.ToString(), ClaimValueTypes.Integer64, "ITAN"));
        }

        public void StoreItanRolesToSession(ClaimsIdentity identity)
        {
            var currentUserId = long.Parse(identity.Claims.Single(x => x.Type == ItanClaimTypes.ApplicationIdentifier).Value);
            var itanRoles = this.entityRepository.GetRolesTypesForUser(currentUserId);
            var claims = this.mapper.Map<List<ItanRole>, List<Claim>>(itanRoles);
            identity.AddClaims(claims);
        }

        private void CreateAndAssignNewSocialLoginForApplicationUser(Claim identifier,
                                                                     AuthenticationTypeProvider authenticationTypeProvider,
                                                                     long newUserId)
        {
            this.entityRepository.CreateNewSociaLogin(identifier.Value, authenticationTypeProvider, newUserId);
        }

        private long CreateNewApplicationUser(ClaimsIdentity identity)
        {
            var name = identity.Claims.Single(x => x.Type == ClaimTypes.Name);
            var email = identity.Claims.Single(x => x.Type == ClaimTypes.Email);
            return this.entityRepository.CreateNewUser(name.Value, email.Value);
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