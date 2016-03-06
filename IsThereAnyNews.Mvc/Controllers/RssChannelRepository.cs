using System.Collections.Generic;
using System.Linq;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class RssChannelRepository
    {
        static private List<RssChannel> listOfChannels = new List<RssChannel>();

        public void AddToGlobalSpace(List<RssChannel> importFromUpload)
        {
            foreach (var channel in importFromUpload)
            {
                var rssChannel = new RssChannel(channel.Url, channel.Title)
                {
                    Id = listOfChannels.Max(x => x.Id) + 1
                };

                listOfChannels.Add(rssChannel);
            }
        }

        public List<RssChannel> LoadAllChannels()
        {
            return listOfChannels;
        }

        public RssChannel Load(long id)
        {
            return listOfChannels.Single(channel => channel.Id == id);
        }
    }
}