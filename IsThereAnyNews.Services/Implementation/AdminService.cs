namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;
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
                    UserRegistration = appConfiguration.RegistrationSupported,
                    UserLimit = appConfiguration.UsersLimit,
                    CurrentUsers = numberOfRegisteredUsers,
                    Subscriptions = numberOfRssSubscriptions,
                    RssNews = numberOfRssNews,
                };
            return x;
        }
    }
}