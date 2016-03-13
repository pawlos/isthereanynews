using System;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.EntityFramework.Models
{
    public sealed class SocialLogin : IModel
    {
        public SocialLogin() : this(string.Empty, AuthenticationTypeProvider.Unknown, -1)
        { }

        public SocialLogin(string identifier, AuthenticationTypeProvider authenticationTypeProvider, long userId)
        {
            this.SocialId = identifier;
            this.Provider = authenticationTypeProvider;
            this.UserId = userId;
        }

        public string SocialId { get; set; }
        public AuthenticationTypeProvider Provider { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}