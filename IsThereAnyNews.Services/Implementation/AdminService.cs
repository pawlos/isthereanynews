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
        private readonly IMapper mapper;

        private readonly IEntityRepository entityRepository;

        public AdminService(IMapper mapper, IEntityRepository entityRepository)
        {
            this.mapper = mapper;
            this.entityRepository = entityRepository;
        }

        public ItanApplicationConfigurationViewModel ReadConfiguration()
        {
            var appConfiguration = this.entityRepository.LoadApplicationConfiguration();
            var numberOfRegisteredUsers = this.entityRepository.GetNumberOfRegisteredUsers();
            var numberOfRssSubscriptions = this.entityRepository.GetNumberOfRssSubscriptions();
            var numberOfRssNews = this.entityRepository.GetNumberOfRssNews();
            var viewmodel = this.mapper.Map<ApplicationConfigurationDTO, ItanApplicationConfigurationViewModel>(appConfiguration);

            viewmodel.CurrentUsers = numberOfRegisteredUsers;
            viewmodel.Subscriptions = numberOfRssSubscriptions;
            viewmodel.RssNews = numberOfRssNews;

            return viewmodel;
        }

        public void ChangeRegistration(ChangeRegistrationDto dto)
        {
            this.entityRepository.ChangeApplicationRegistration(dto.Status);
        }

        public void ChangeUsersLimit(ChangeUsersLimitDto dto)
        {
            this.entityRepository.ChangeUserLimit(dto.Limit);
        }
    }
}