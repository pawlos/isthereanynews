namespace IsThereAnyNews.Services.Handlers.Implementation
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.Services.Handlers.ViewModels;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class PersonSubscriptionHandler : ISubscriptionHandler
    {
        private readonly IEntityRepository entityRepository;

        private readonly IMapper mapper;

        public PersonSubscriptionHandler(
            IMapper mapper,
            IEntityRepository entityRepository)
        {
            this.mapper = mapper;
            this.entityRepository = entityRepository;
        }

        public ISubscriptionContentIndexViewModel GetSubscriptionViewModel(
            long userId,
            long subscriptionId,
            ShowReadEntries showReadEntries)
        {
            if (!this.entityRepository.DoesUserOwnsUserSubscription(subscriptionId, userId))
            {
                var rssSubscriptionIndexViewModel = new PersonSubscriptionIndexViewModel(
                        subscriptionId,
                        "You are not subscribed to this user",
                        DateTime.MaxValue,
                        new List<RssEntryToReadViewModel>());
                return rssSubscriptionIndexViewModel;
            }

            List<UserSubscriptionEntryToReadDTO> loadAllUnreadEntriesFromSubscription;
            if (showReadEntries == ShowReadEntries.Show)
            {
                loadAllUnreadEntriesFromSubscription =
                        this.entityRepository.LoadAllUserEntriesFromSubscription(subscriptionId);
            }
            else
            {
                loadAllUnreadEntriesFromSubscription =
                        this.entityRepository.LoadAllUserUnreadEntriesFromSubscription(subscriptionId);
            }

            var channelInformation = this.entityRepository.LoadUserChannelInformation(subscriptionId);

            var rssEntryToReadViewModels =
                    this.mapper.Map<List<UserSubscriptionEntryToReadDTO>, List<RssEntryToReadViewModel>>
                            (loadAllUnreadEntriesFromSubscription);
            rssEntryToReadViewModels.ForEach(x=>x.RssEntryViewModel.SubscriptionId = subscriptionId);

            var viewModel = new PersonSubscriptionIndexViewModel(
                    subscriptionId,
                    channelInformation.Title,
                    channelInformation.Created,
                    rssEntryToReadViewModels);

            return viewModel;
        }

        public void MarkNavigated(long userId, long rssId, long dtoSubscriptionId)
        {
            this.entityRepository.MarkPersonActivityNavigated(rssId, dtoSubscriptionId);
            this.entityRepository.AddEventPersonActivityNavigated(userId, rssId);
        }

        public void MarkClicked(long cui, long id, long subscriptionId)
        {
            this.entityRepository.MarkPersonActivityClicked(id, subscriptionId);
            this.entityRepository.AddEventPersonActivityClicked(cui, id);
        }

        public void MarkSkipped(long cui, long subscriptionId, List<long> entries)
        {
            this.entityRepository.MarkPersonActivitySkipped(subscriptionId, entries);
            this.entityRepository.AddEventPersonActivitySkipped(cui, entries);
        }
    }
}