namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.ViewModels;

    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUsersSubscriptionRepository userSubscriptionsRepository;
        private readonly IUserSubscriptionEntryToReadRepository userSubscriptionsEntryToReadRepository;

        private readonly IUserAuthentication authentication;

        public UserSubscriptionService(IUsersSubscriptionRepository userSubscriptionsRepository,
            IUserSubscriptionEntryToReadRepository userSubscriptionsEntryToReadRepository,
            IUserAuthentication authentication)
        {
            this.userSubscriptionsRepository = userSubscriptionsRepository;
            this.userSubscriptionsEntryToReadRepository = userSubscriptionsEntryToReadRepository;
            this.authentication = authentication;
        }

        public List<ObservableUserEventsInformation> LoadAllObservableSubscription()
        {
            DateTime now = DateTime.Now;
            var currentUserId = this.authentication.GetCurrentUserId();
            this.userSubscriptionsEntryToReadRepository.CopyAllUnreadElementsToUser(currentUserId);
            var loadNameAndCountForUser = this.userSubscriptionsRepository.LoadNameAndCountForUser(currentUserId);
            this.userSubscriptionsRepository.UpdateUserLastReadTime(currentUserId, now);
            var list = loadNameAndCountForUser.Select(this.ProjectToObservableUserEventsInformation).ToList();
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