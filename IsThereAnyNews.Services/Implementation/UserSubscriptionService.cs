namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.ViewModels;

    public class UserSubscriptionService : IUserSubscriptionService
    {
        private readonly IUserAuthentication authentication;
        private readonly IEntityRepository entityRepository;

        public UserSubscriptionService(
            IUserAuthentication authentication, IEntityRepository entityRepository)
        {
            this.authentication = authentication;
            this.entityRepository = entityRepository;
        }

        public List<ObservableUserEventsInformation> LoadAllObservableSubscription()
        {
            DateTime now = DateTime.Now;
            var currentUserId = this.authentication.GetCurrentUserId();
            this.entityRepository.CopyAllUnreadElementsToUser(currentUserId);
            var loadNameAndCountForUser = this.entityRepository.LoadNameAndCountForUser(currentUserId);
            this.entityRepository.UpdateUserLastReadTime(currentUserId, now);
            var list = loadNameAndCountForUser.Select(this.ProjectToObservableUserEventsInformation).ToList();
            return list;
        }

        private ObservableUserEventsInformation ProjectToObservableUserEventsInformation(Dtos.NameAndCountUserSubscription arg)
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