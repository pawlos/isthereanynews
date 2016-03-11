using System.Collections.Generic;

namespace IsThereAnyNews.EntityFramework.Models
{
    public sealed class ItanUser
    {
        public ItanUser(string identifier, string name)
        {
            this.Id = identifier;
            this.Name = name;
            this.RssChannels = new List<RssChannel>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public List<RssChannel> RssChannels { get; set; }
    }
}