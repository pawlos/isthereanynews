namespace IsThereAnyNews.Services.Implementation
{
    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;

        private readonly IMapper mapper;

        private readonly IUserAuthentication authentication;

        public AccountService(IUserRepository userRepository, IMapper mapper, IUserAuthentication authentication)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.authentication = authentication;
        }

        public AccountDetailsViewModel GetAccountDetailsViewModel()
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            var userPrivateDetails = this.userRepository.GetUserPrivateDetails(currentUserId);
            var accountDetailsViewModel = this.mapper.Map<AccountDetailsViewModel>(userPrivateDetails);
            return accountDetailsViewModel;
        }

        public void ChangeEmail(ChangeEmailModelDto model)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            this.userRepository.ChangeEmail(currentUserId, model.Email);
        }

        public void ChangeDisplayName(ChangeDisplayNameModelDto model)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            this.userRepository.ChangeDisplayName(currentUserId, model.Displayname);
        }
    }
}