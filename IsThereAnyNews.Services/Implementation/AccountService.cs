namespace IsThereAnyNews.Services.Implementation
{
    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public class AccountService : IAccountService
    {
        private readonly ISessionProvider sessionProvider;

        private readonly IUserRepository userRepository;

        private readonly IMapper mapper;

        public AccountService(ISessionProvider sessionProvider, IUserRepository userRepository, IMapper mapper)
        {
            this.sessionProvider = sessionProvider;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public AccountDetailsViewModel GetAccountDetailsViewModel()
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            var userPrivateDetails = this.userRepository.GetUserPrivateDetails(currentUserId);
            var accountDetailsViewModel = this.mapper.Map<AccountDetailsViewModel>(userPrivateDetails);
            return accountDetailsViewModel;
        }

        public void ChangeEmail(ChangeEmailModelDto model)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            this.userRepository.ChangeEmail(currentUserId, model.Email);
        }

        public void ChangeDisplayName(ChangeDisplayNameModelDto model)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            this.userRepository.ChangeDisplayName(currentUserId, model.Displayname);
        }
    }
}