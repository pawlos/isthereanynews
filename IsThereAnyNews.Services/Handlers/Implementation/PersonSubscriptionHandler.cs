using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.Services.Handlers.Implementation
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public class PersonSubscriptionHandler: ISubscriptionHandler
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

        public void AddEventSkipped(long cui, List<long> entries)
        {
            // dont know what to do here
        }

        public void AddEventViewed(long cui, long id)
        {
            // dont know what to do here
        }

        public ISubscriptionContentIndexViewModel GetSubscriptionViewModel(
            long userId,
            long subscriptionId,
            ShowReadEntries showReadEntries)
        {
            return this.GetPersonSubscriptionIndexViewModel(userId, subscriptionId, showReadEntries);
        }

        public void MarkRead(long userId, long rssId, long dtoSubscriptionId)
        {
            // empty now
        }

        public void MarkSkipped(long modelSubscriptionId, List<long> ids)
        {
            this.entityRepository.MarkPersonEntriesSkipped(modelSubscriptionId, ids);
        }

        private ISubscriptionContentIndexViewModel GetPersonSubscriptionIndexViewModel(
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
            if(showReadEntries != ShowReadEntries.Show)
            {
                loadAllUnreadEntriesFromSubscription =
                    this.entityRepository.LoadAllUserUnreadEntriesFromSubscription(subscriptionId);
            }
            else
            {
                loadAllUnreadEntriesFromSubscription =
                    this.entityRepository.LoadAllUserEntriesFromSubscription(subscriptionId);
            }

            var channelInformation = this.entityRepository.LoadUserChannelInformation(subscriptionId);

            var rssEntryToReadViewModels =
                    this.mapper.Map<List<UserSubscriptionEntryToReadDTO>, List<RssEntryToReadViewModel>>
                    (loadAllUnreadEntriesFromSubscription);

            var viewModel = new PersonSubscriptionIndexViewModel(
                subscriptionId,
                channelInformation.Title,
                channelInformation.Created,
                rssEntryToReadViewModels);

            return viewModel;
        }
    }
}