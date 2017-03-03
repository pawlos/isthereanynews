namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class SystemSubscriptionService : ISystemSubscriptionService
    {
        private readonly IUserAuthentication authentication;
        private readonly IMapper mapper;

        private readonly IEntityRepository entityRepository;

        public SystemSubscriptionService(
            IUserAuthentication authentication,
            IMapper mapper, 
            IEntityRepository entityRepository)
        {
            this.authentication = authentication;
            this.mapper = mapper;
            this.entityRepository = entityRepository;
        }

        public AdminEventsViewModel LoadEvents()
        {
            var roles = this.authentication.GetCurrentUserRoles();
            if (roles.Contains(ItanRole.SuperAdmin))
            {
                return this.LoadSuperAdminEvents();
            }

            return new AdminEventsViewModel();
        }

        public List<ChannelEventViewModel> LoadAdminEvents()
        {
            return new List<ChannelEventViewModel>();
        }

        public AdminEventsViewModel LoadSuperAdminEvents()
        {
            var updates = this.entityRepository.LoadUpdateEvents();
            var creations = this.entityRepository.LoadCreateEvents();
            var exceptions = this.entityRepository.LoadExceptionEvents();

            ChannelEventViewModel u = new ChannelEventUpdatesViewModel
            {
                Count = updates.UpdateCout.ToString(),
                Name = "Updates",
                Id = -1
            };

            var c = new ChannelEventCreationViewModel
            {
                Count = creations.Count.ToString(),
                Name = "Creations",
                Id = -2
            };

            var e = new ChannelEventExceptionViewModel
            {
                Count = exceptions.Count.ToString(),
                Name = "Exceptions",
                Id = -3
            };

            var events = new AdminEventsViewModel
            {
                Updates = u,
                Creations = c,
                Exceptions = e
            };


            return events;
        }
    }
}