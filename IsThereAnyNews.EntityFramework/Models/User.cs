using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace IsThereAnyNews.EntityFramework.Models
{
    public sealed class User : IModel
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
    }
}