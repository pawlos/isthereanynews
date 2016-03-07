namespace IsThereAnyNews.Mvc.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;

    public sealed class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string identifier, string name)
        {
            this.Id = identifier;
            this.UserName = name.Replace(" ", string.Empty);
            this.RssChannels = new List<RssChannel>();
        }

        public List<RssChannel> RssChannels { get; set; }
    }
}