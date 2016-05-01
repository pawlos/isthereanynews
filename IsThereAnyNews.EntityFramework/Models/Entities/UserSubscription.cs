using System;
using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models.Events;
using IsThereAnyNews.EntityFramework.Models.Interfaces;

namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    public class UserSubscription : IEntity, ICreatable, IModifiable, IEqualityComparer<UserSubscription>
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public long FollowerId { get; set; }
        public User Follower { get; set; }

        public long ObservedId { get; set; }
        public User Observed { get; set; }

        public List<UserSubscriptionEntryToRead> EntriesToRead { get; set; }
        public bool Equals(UserSubscription x, UserSubscription y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(UserSubscription obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}