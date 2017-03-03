namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class PersonSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IUserAuthentication authentication;

        private readonly IEntityRepository entityRepository;

        private readonly IMapper mapper;

        public PersonSubscriptionHandler(
            IMapper mapper,
            IUserAuthentication authentication,
            IEntityRepository entityRepository)
        {
            this.mapper = mapper;
            this.authentication = authentication;
            this.entityRepository = entityRepository;
        }

        public void AddEventSkipped(long cui, string entries)
        {
            // dont know what to do here
        }

        public void AddEventViewed(long dtoId)
        {
            // do nothing for now
        }

        public void AddEventViewed(long cui, long id)
        {
            // dont know what to do here
        }

        public RssSubscriptionIndexViewModel GetSubscriptionViewModel(
            long subscriptionId,
            ShowReadEntries showReadEntries)
        {
            return this.GetPersonSubscriptionIndexViewModel(subscriptionId, showReadEntries);
        }

        public void MarkRead(long userId, long rssId, long dtoSubscriptionId)
        {
            throw new NotImplementedException();
        }

        public void MarkRead(List<long> displayedItems)
        {
            throw new NotImplementedException();
        }

        public void MarkSkipped(long modelSubscriptionId, List<long> ids)
        {
            this.entityRepository.MarkPersonEntriesSkipped(modelSubscriptionId, ids);
        }

        private RssSubscriptionIndexViewModel GetPersonSubscriptionIndexViewModel(
            long subscriptionId,
            ShowReadEntries showReadEntries)
        {
            var currentUserId = this.authentication.GetCurrentUserId();

            if (!this.entityRepository.DoesUserOwnsSubscription(subscriptionId, currentUserId))
            {
                var ci = new ChannelInformationViewModel
                             {
                                 Title = "You are not subscribed to this user",
                                 Created = DateTime.MaxValue
                             };

                var rssSubscriptionIndexViewModel = new RssSubscriptionIndexViewModel(
                    subscriptionId,
                    ci,
                    new List<RssEntryToReadViewModel>(),
                    StreamType.Person);
                return rssSubscriptionIndexViewModel;
            }

            List<UserSubscriptionEntryToReadDTO> loadAllUnreadEntriesFromSubscription;
            if (showReadEntries != ShowReadEntries.Show)
            {
                loadAllUnreadEntriesFromSubscription =
                    this.entityRepository.LoadAllUserUnreadEntriesFromSubscription(subscriptionId);
            }
            else
            {
                loadAllUnreadEntriesFromSubscription =
                    this.entityRepository.LoadAllUserEntriesFromSubscription(subscriptionId);
            }

            var channelInformation = this.entityRepository.LoadChannelInformation(subscriptionId);

            var channelInformationViewModel = new ChannelInformationViewModel
                                                  {
                                                      Title = channelInformation.Title,
                                                      Created = channelInformation.Created
                                                  };

            var rssEntryToReadViewModels =
                this.mapper.Map<List<RssEntryToReadViewModel>>(loadAllUnreadEntriesFromSubscription);

            var viewModel = new RssSubscriptionIndexViewModel(
                subscriptionId,
                channelInformationViewModel,
                rssEntryToReadViewModels,
                StreamType.Person);

            return viewModel;
        }
    }
}