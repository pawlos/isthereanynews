using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssEntriesToReadRepository : IRssEntriesToReadRepository
    {
        private readonly ItanDatabaseContext database;

        public RssEntriesToReadRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void CopyRssThatWerePublishedAfterLastReadTimeToUser(long currentUserId, List<RssChannelSubscription> subscriptions)
        {
            var rssChannelsIds = subscriptions.Select(x => x.RssChannelId).ToList();

            var rssEntries = this.database.RssEntries
                .Where(e => rssChannelsIds.Contains(e.RssChannelId))
                .Include(e => e.RssEntryToRead)
                .Where(e => e.RssEntryToRead == null)
                .ToList();


            var toread = new List<RssEntryToRead>(rssEntries.Count);

            rssEntries.ForEach(entry =>
            {
                var rssEntryToRead = new RssEntryToRead(
                    entry,
                    subscriptions.Single(x => x.RssChannelId == entry.RssChannelId).Id);
                toread.Add(rssEntryToRead);
            });

            this.database.RssEntriesToRead.AddRange(toread);
            this.database.SaveChanges();

        }
    }
}