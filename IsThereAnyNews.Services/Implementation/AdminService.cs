namespace IsThereAnyNews.Services.Implementation
{
    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.ViewModels;

    public class AdminService : IAdminService
    {
        private readonly IAdminRepository adminRepository;

        private readonly IMapper mapper;

        public AdminService(IAdminRepository adminRepository, IMapper mapper)
        {
            this.adminRepository = adminRepository;
            this.mapper = mapper;
        }

        public ItanApplicationConfigurationViewModel ReadConfiguration()
        {
            var appConfiguration = this.adminRepository.LoadApplicationConfiguration();
            var numberOfRegisteredUsers = this.adminRepository.GetNumberOfRegisteredUsers();
            var numberOfRssSubscriptions = this.adminRepository.GetNumberOfRssSubscriptions();
            var numberOfRssNews = this.adminRepository.GetNumberOfRssNews();
            var viewmodel = this.mapper.Map<ApplicationConfigurationDTO, ItanApplicationConfigurationViewModel>(appConfiguration);

            viewmodel.CurrentUsers = numberOfRegisteredUsers;
            viewmodel.Subscriptions = numberOfRssSubscriptions;
            viewmodel.RssNews = numberOfRssNews;

            return viewmodel;
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