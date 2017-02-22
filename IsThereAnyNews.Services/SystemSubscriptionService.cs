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

        public List<ChannelEventViewModel> LoadEvents()
        {
            var roles = this.authentication.GetCurrentUserRoles();
            if (roles.Contains(ItanRole.SuperAdmin))
            {
                return this.LoadSuperAdminEvents();
            }
            if (roles.Contains(ItanRole.Admin))
            {
                return this.LoadAdminEvents();
            }
            return new List<ChannelEventViewModel>();
        }

        public List<ChannelEventViewModel> LoadAdminEvents()
        {
            return new List<ChannelEventViewModel>();
        }

        public List<ChannelEventViewModel> LoadSuperAdminEvents()
        {
            List<ChannelEventViewModel> events = new List<ChannelEventViewModel>();
            var updates = this.rssChannelRepository.LoadUpdateEvents();
            var creations = this.rssChannelRepository.LoadCreateEvents();
            ChannelEventViewModel u = new ChannelEventViewModel
            {
                Count = updates.UpdateCout.ToString(),
                Name = "Updates"
            };

            var c = new ChannelEventViewModel
            {
                Count = creations.Count.ToString(),
                Name = "Creations"
            };

            events.Add(u);
            events.Add(c);

            return events;
        }
    }
}