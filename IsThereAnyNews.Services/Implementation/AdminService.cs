namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.ViewModels;

    public class AdminService : IAdminService
    {
        private readonly IAdminRepository adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        public ItanApplicationConfigurationViewModel ReadConfiguration()
        {
            var appConfiguration = this.adminRepository.LoadApplicationConfiguration();
            var numberOfRegisteredUsers = this.adminRepository.GetNumberOfRegisteredUsers();
            var numberOfRssSubscriptions = this.adminRepository.GetNumberOfRssSubscriptions();
            var numberOfRssNews = this.adminRepository.GetNumberOfRssNews();

            var x =
                new ItanApplicationConfigurationViewModel
                {
                    UserRegistration = appConfiguration.RegistrationSupported.ToString(),
                    UserLimit = appConfiguration.UsersLimit,
                    CurrentUsers = numberOfRegisteredUsers,
                    Subscriptions = numberOfRssSubscriptions,
                    RssNews = numberOfRssNews,
                };
            return x;
        }

        public void ChangeRegistration(ChangeRegistrationDto dto)
        {
            this.adminRepository.ChangeApplicationRegistration(dto.Status);
        }

        public void ChangeUsersLimit(ChangeUsersLimitDto dto)
        {
            this.adminRepository.ChangeUserLimit(dto.Limit);
        }
    }
}