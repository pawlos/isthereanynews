namespace IsThereAnyNews.Services.Implementation
{
    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IUserAuthentication authentication;
        private readonly IEntityRepository entityRepository;

        public AccountService(IMapper mapper, IUserAuthentication authentication, IEntityRepository entityRepository)
        {
            this.mapper = mapper;
            this.authentication = authentication;
            this.entityRepository = entityRepository;
        }

        public AccountDetailsViewModel GetAccountDetailsViewModel()
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            var userPrivateDetails = this.entityRepository.GetUserPrivateDetails(currentUserId);
            var accountDetailsViewModel = this.mapper.Map<AccountDetailsViewModel>(userPrivateDetails);
            return accountDetailsViewModel;
        }

        public void ChangeEmail(ChangeEmailModelDto model)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            this.entityRepository.ChangeEmail(currentUserId, model.Email);
        }

        public void ChangeDisplayName(ChangeDisplayNameModelDto model)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            this.entityRepository.ChangeDisplayName(currentUserId, model.Displayname);
        }
    }
}