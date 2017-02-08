namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class PersonSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IRssEventRepository rssEventsRepository;
        private readonly IUsersSubscriptionRepository usersSubscriptionRepository;
        private readonly IMapper mapper;

        private readonly IUserAuthentication authentication;

        public PersonSubscriptionHandler(
            IRssEventRepository rssEventsRepository,
            IUsersSubscriptionRepository usersSubscriptionRepository,
            IMapper mapper,
            IUserAuthentication authentication)
        {
            this.rssEventsRepository = rssEventsRepository;
            this.usersSubscriptionRepository = usersSubscriptionRepository;
            this.mapper = mapper;
            this.authentication = authentication;
        }

        public RssSubscriptionIndexViewModel GetSubscriptionViewModel(long subscriptionId, ShowReadEntries showReadEntries)
        {
            return this.GetPersonSubscriptionIndexViewModel(subscriptionId, showReadEntries);
        }

        public void MarkRead(List<long> displayedItems)
        {
            this.rssEventsRepository.MarkRead(displayedItems);
        }

        public void AddEventViewed(long dtoId)
        {
            // do nothing for now
        }

        private RssSubscriptionIndexViewModel GetPersonSubscriptionIndexViewModel(long subscriptionId, ShowReadEntries showReadEntries)
        {
            var currentUserId = this.authentication.GetCurrentUserId();

            if (!this.usersSubscriptionRepository.DoesUserOwnsSubscription(subscriptionId, currentUserId))
            {
                var ci = new ChannelInformationViewModel
                {
                    Title = "You are not subscribed to this user",
                    Created = DateTime.MaxValue
                };

                var rssSubscriptionIndexViewModel =
                    new RssSubscriptionIndexViewModel(subscriptionId,
                        ci,
                        new List<RssEntryToReadViewModel>(),
                        StreamType.Person);
                return rssSubscriptionIndexViewModel;
            }

            List<UserSubscriptionEntryToReadDTO> loadAllUnreadEntriesFromSubscription;
            if (showReadEntries != ShowReadEntries.Show)
            {
                loadAllUnreadEntriesFromSubscription = this.usersSubscriptionRepository
                    .LoadAllUnreadEntriesFromSubscription(subscriptionId);
            }
            else
            {
                loadAllUnreadEntriesFromSubscription = this.usersSubscriptionRepository
                    .LoadAllEntriesFromSubscription(subscriptionId);
            }

            var channelInformation =
                this.usersSubscriptionRepository
                    .LoadChannelInformation(subscriptionId);

            var channelInformationViewModel = new ChannelInformationViewModel
            {
                Title = channelInformation.Title,
                Created = channelInformation.Created
            };

            var rssEntryToReadViewModels = this.mapper.Map<List<RssEntryToReadViewModel>>(loadAllUnreadEntriesFromSubscription);

            var viewModel = new RssSubscriptionIndexViewModel(
                subscriptionId,
                channelInformationViewModel,
                rssEntryToReadViewModels,
                StreamType.Person);

            return viewModel;
        }

        public void MarkSkipped(long modelSubscriptionId, List<long> ids)
        {
            this.usersSubscriptionRepository.MarkEntriesSkipped(modelSubscriptionId, ids);
        }

        public void MarkRead(long userId, long rssId, long dtoSubscriptionId)
        {
            throw new NotImplementedException();
        }

        public void AddEventViewed(long cui, long id)
        {
            // dont know what to do here
        }

        public void AddEventSkipped(long cui, string entries)
        {
            // dont know what to do here
        }
    }
}