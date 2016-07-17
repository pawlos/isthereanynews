namespace IsThereAnyNews.Services.Implementation
{
    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Mvc.Controllers;
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
    }
}