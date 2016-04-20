using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using IsThereAnyNews.EntityFramework.Models.Events;
using IsThereAnyNews.EntityFramework.Models.Interfaces;

namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    public sealed class User : IEntity, ICreatable, IModifiable, IEqualityComparer<User>
    {
        public User()
        {
            this.LastReadTime = SqlDateTime.MinValue.Value;
        }

        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }

        public DateTime LastReadTime { get; set; }

        public List<SocialLogin> SocialLogins { get; set; }
        public List<RssChannelSubscription> RssSubscriptionList { get; set; }
        public List<EventRssViewed> EventsRssViewed { get; set; }

        public bool Equals(User x, User y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(User obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}