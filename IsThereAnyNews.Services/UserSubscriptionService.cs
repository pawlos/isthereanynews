using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly ISessionProvider sessionProvider;
        private readonly IUsersSubscriptionRepository userSubscriptionsRepository;
        private readonly IUserSubscriptionEntryToReadRepository userSubscriptionsEntryToReadRepository;

        public UserSubscriptionService(
            ISessionProvider sessionProvider,
            IUsersSubscriptionRepository userSubscriptionsRepository,
            IUserSubscriptionEntryToReadRepository userSubscriptionsEntryToReadRepository)
        {
            this.sessionProvider = sessionProvider;
            this.userSubscriptionsRepository = userSubscriptionsRepository;
            this.userSubscriptionsEntryToReadRepository = userSubscriptionsEntryToReadRepository;
        }

        public List<ObservableUserEventsInformation> LoadAllObservableSubscription()
        {
            DateTime now = DateTime.Now;
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            this.userSubscriptionsEntryToReadRepository.CopyAllUnreadElementsToUser(currentUserId);
            var loadNameAndCountForUser = this.userSubscriptionsRepository.LoadNameAndCountForUser(currentUserId);
            this.userSubscriptionsRepository.UpdateUserLastReadTime(currentUserId,now);
            var list = loadNameAndCountForUser.Select(ProjectToObservableUserEventsInformation).ToList();
            return list;
        }

        private ObservableUserEventsInformation ProjectToObservableUserEventsInformation(NameAndCountUserSubscription arg)
        {
            return new ObservableUserEventsInformation
            {
                Id = arg.Id,
                Name = arg.Name,
                Count = arg.Count.ToString()
            };
        }
    }
}