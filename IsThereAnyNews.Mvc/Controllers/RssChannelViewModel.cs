using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssChannelViewModel
    {
        public RssChannelViewModel(RssChannel channel)
        {
            this.Id = channel.Id;
            this.Title = channel.Title;
            this.Url = channel.Url;
        }

        public string Url { get; set; }
        public string Title { get; set; }
        public long Id { get; set; }
    }
}