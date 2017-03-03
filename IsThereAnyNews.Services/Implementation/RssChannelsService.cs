using IsThereAnyNews.ViewModels.RssChannel;

namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.ViewModels;

    public class RssChannelsService : IRssChannelsService
    {
        private readonly IUserAuthentication authentication;
        private readonly IMapper mapping;
        private readonly IUpdateService updateService;
        private IEntityRepository entityRepository;

        public RssChannelsService(
            IUserAuthentication authentication,
            IMapper mapping,
            IUpdateService updateService, 
            IEntityRepository entityRepository)
        {
            this.authentication = authentication;
            this.mapping = mapping;
            this.updateService = updateService;
            this.entityRepository = entityRepository;
        }

        public RssChannelsIndexViewModel LoadAllChannels()
        {
            var allChannels = this.entityRepository.LoadAllChannelsWithStatistics();
            var viewmodel = this.mapping.Map<RssChannelsIndexViewModel>(allChannels);
            return viewmodel;
        }

        public RssChannelsMyViewModel LoadAllChannelsOfCurrentUser()
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            var rssSubscriptions = this.entityRepository.LoadAllSubscriptionsForUser(currentUserId);
//            this.rssEntriesToReadRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(currentUserId, rssSubscriptions);
            var viewmodel = this.mapping.Map<List<RssChannelSubscriptionDTO>, RssChannelsMyViewModel>(rssSubscriptions);
            return viewmodel;
        }

        public RssChannelIndexViewModel GetViewModelFormChannelId(long id)
        {
            var rssChannel = this.entityRepository.LoadRssChannel(id);
            var rssChannelIndexViewModel =
                this.mapping.Map<RssChannelDTO, RssChannelIndexViewModel>(
                rssChannel,
                o => o.AfterMap(
                    (s, d) =>
                        {
                            d.Entries = d.Entries.OrderByDescending(item => item.PublicationDate).ToList();
                        }));

            if (!this.authentication.CurrentUserIsAuthenticated())
            {
                return rssChannelIndexViewModel;
            }

            rssChannelIndexViewModel.IsAuthenticatedUser = true;
            var userRssSubscriptionInfoViewModel = this.CreateUserSubscriptionInfo(id);
            rssChannelIndexViewModel.SubscriptionInfo = userRssSubscriptionInfoViewModel;

            return rssChannelIndexViewModel;
        }

        public UserRssSubscriptionInfoViewModel CreateUserSubscriptionInfo(long id)
        {
            var userId = this.authentication.GetCurrentUserId();
            var subscriptionInfo = this.entityRepository.FindSubscriptionIdOfUserAndOfChannel(userId, id);
            var userRssSubscriptionInfoViewModel = this.mapping.Map<UserRssSubscriptionInfoViewModel>(subscriptionInfo);
            return userRssSubscriptionInfoViewModel;
        }

        public void CreateNewChannelIfNotExists(AddChannelDto dto)
        {
            var idByChannelUrl = this.entityRepository.GetIdByChannelUrl(new List<string> { dto.RssChannelLink });
            if (!idByChannelUrl.Any())
            {
                this.CreateNewChannel(dto);
            }
        }

        private void CreateNewChannel(AddChannelDto dto)
        {
            var rssSourceWithUrlAndTitles = new List<RssSourceWithUrlAndTitle>
                                                {
                                                    new RssSourceWithUrlAndTitle(
                                                        dto.RssChannelLink,
                                                        dto.RssChannelName)
                                                };
            this.entityRepository.SaveToDatabase(rssSourceWithUrlAndTitles);

            var urlsToChannels = new List<string> { dto.RssChannelLink };
            var listIds = this.entityRepository.GetIdByChannelUrl(urlsToChannels);
            var id = listIds.Single();

            this.entityRepository.SaveChannelCreatedEventToDatabase(id);
        }
    }
}