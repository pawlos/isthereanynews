using System.Collections.Generic;
using AutoMapper;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.SharedData;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public class SystemSubscriptionService : ISystemSubscriptionService
    {
        private readonly IUserAuthentication authentication;
        private readonly IRssChannelsRepository rssChannelRepository;
        private readonly IMapper mapper;

        public SystemSubscriptionService(
            IUserAuthentication authentication,
            IRssChannelsRepository rssChannelRepository,
            IMapper mapper)
        {
            this.authentication = authentication;
            this.rssChannelRepository = rssChannelRepository;
            this.mapper = mapper;
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
            var updates = this.rssChannelRepository.LoadUpdateEvents();
            var creations = this.rssChannelRepository.LoadCreateEvents();
            var exceptions = this.rssChannelRepository.LoadExceptionEvents();

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

    public class ChannelEventExceptionViewModel : ChannelEventViewModel
    {
        public override StreamType StreamType => StreamType.Exception;
    }

    public class ChannelEventCreationViewModel : ChannelEventViewModel
    {
        public override StreamType StreamType => StreamType.Channel;
    }

    public class ChannelEventUpdatesViewModel : ChannelEventViewModel
    {
        public override StreamType StreamType => StreamType.Channel;
    }
}