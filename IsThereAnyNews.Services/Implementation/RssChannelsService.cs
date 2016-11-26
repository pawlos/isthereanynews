namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.ViewModels;

    public class RssChannelsService : IRssChannelsService
    {
        private readonly IRssChannelsRepository channelsRepository;

        private readonly IRssChannelsSubscriptionsRepository channelsSubscriptionRepository;
        private readonly IRssEntriesToReadRepository rssEntriesToReadRepository;
        private readonly IUserAuthentication authentication;
        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionRepository;

        private readonly IMapper mapping;
        private readonly IEventRssChannelCreatedRepository eventRssChannelCreatedRepository;

        private readonly IUpdateService updateService;

        public RssChannelsService(
            IRssChannelsRepository channelsRepository,
            IRssChannelsSubscriptionsRepository channelsSubscriptionRepository,
            IRssEntriesToReadRepository rssEntriesToReadRepository,
            IUserAuthentication authentication,
            IRssChannelsSubscriptionsRepository rssSubscriptionRepository,
            IMapper mapping,
            IEventRssChannelCreatedRepository eventRssChannelCreatedRepository,
            IUpdateService updateService)
        {
            this.channelsRepository = channelsRepository;
            this.channelsSubscriptionRepository = channelsSubscriptionRepository;
            this.rssEntriesToReadRepository = rssEntriesToReadRepository;
            this.authentication = authentication;
            this.rssSubscriptionRepository = rssSubscriptionRepository;
            this.mapping = mapping;
            this.eventRssChannelCreatedRepository = eventRssChannelCreatedRepository;
            this.updateService = updateService;
        }

        public RssChannelsIndexViewModel LoadAllChannels()
        {
            var allChannels = this.channelsRepository.LoadAllChannelsWithStatistics();
            var viewmodel = this.mapping.Map<RssChannelsIndexViewModel>(allChannels);
            return viewmodel;
        }

        public RssChannelsMyViewModel LoadAllChannelsOfCurrentUser()
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            var rssSubscriptions = this.channelsSubscriptionRepository.LoadAllSubscriptionsForUser(currentUserId);
            this.rssEntriesToReadRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(currentUserId, rssSubscriptions);
            var viewmodel = this.mapping.Map<List<RssChannelSubscription>, RssChannelsMyViewModel>(rssSubscriptions);
            return viewmodel;
        }

        public RssChannelIndexViewModel GetViewModelFormChannelId(long id)
        {
            var rssChannel = this.channelsRepository.LoadRssChannel(id);
            var rssChannelIndexViewModel =
                this.mapping.Map<RssChannel, RssChannelIndexViewModel>(
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
            var subscriptionInfo = this.rssSubscriptionRepository.FindSubscriptionIdOfUserAndOfChannel(userId, id);
            var userRssSubscriptionInfoViewModel = this.mapping.Map<UserRssSubscriptionInfoViewModel>(subscriptionInfo);
            return userRssSubscriptionInfoViewModel;
        }

        public void CreateNewChannelIfNotExists(AddChannelDto dto)
        {
            var idByChannelUrl = this.channelsRepository.GetIdByChannelUrl(new List<string> { dto.RssChannelLink });
            if (!idByChannelUrl.Any())
            {
                CreateNewChannel(dto);
            }
        }

        private void CreateNewChannel(AddChannelDto dto)
        {
            var rssChannel = this.mapping.Map<RssChannel>(dto);
            this.channelsRepository.SaveToDatabase(new List<RssSourceWithUrlAndTitle> { new RssSourceWithUrlAndTitle(dto.RssChannelLink, dto.RssChannelName) });
            var listIds = this.channelsRepository.GetIdByChannelUrl(new List<string> { rssChannel.Url });
            var id = listIds.Single();

            var eventRssChannelCreated = new EventRssChannelCreated
            {
                RssChannelId = id
            };

            this.eventRssChannelCreatedRepository.SaveToDatabase(eventRssChannelCreated);

            this.updateService.UpdateChannel(rssChannel);
        }
    }
}