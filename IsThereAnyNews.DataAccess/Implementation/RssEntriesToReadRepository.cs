namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using AutoMapper.QueryableExtensions;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;

    public class RssEntriesToReadRepository : IRssEntriesToReadRepository
    {
        private readonly ItanDatabaseContext database;

        public RssEntriesToReadRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void CopyRssThatWerePublishedAfterLastReadTimeToUser(long currentUserId, List<RssChannelSubscriptionDTO> subscriptions)
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

        public void MarkAllReadForUserAndSubscription(long subscriptionId, List<long> id)
        {
            this.database.RssEntriesToRead
                .Where(r => r.RssChannelSubscriptionId == subscriptionId)
                .Where(r => id.Contains(r.Id))
                .Include(r => r.RssEntry)
                .ToList()
                .ForEach(r => r.IsRead = true);
            this.database.SaveChanges();
        }

        public List<RssEntryToReadDTO> LoadAllUnreadEntriesFromSubscription(long subscriptionId)
        {
            var rssEntryToReads = from rssToRead in this.database.RssEntriesToRead
                                  join rss in this.database.RssEntries on rssToRead.RssEntryId equals rss.Id
                                  where rssToRead.RssChannelSubscriptionId == subscriptionId && rssToRead.IsRead == false
                                  select
                                  new RssEntryToReadDTO
                                  {
                                      Id = rssToRead.Id,
                                      IsRead = false,
                                      RssEntryDto =
                                              new RssEntryDTO
                                              {
                                                  Id = rss.Id,
                                                  PreviewText = rss.PreviewText,
                                                  Url = rss.Url,
                                                  Title = rss.Title,
                                                  PublicationDate = rss.PublicationDate
                                              }
                                  };

            var list = rssEntryToReads.ToList();

            return list;
        }

        public List<RssEntryToReadDTO> LoadAllEntriesFromSubscription(long subscriptionId)
        {
            var rssEntryToReads = this.database.RssEntriesToRead
               .Where(r => r.RssChannelSubscriptionId == subscriptionId)
               .Include(r => r.RssEntry)
               .ProjectTo<RssEntryToReadDTO>()
               .ToList();

            return rssEntryToReads;
        }

        public void MarkEntryViewedByUser(long currentUserId, long rssToReadId)
        {
            var rssEntryToRead = this.database
                .RssEntriesToRead
                .Include(r => r.RssChannelSubscription)
                .Include(r => r.RssEntry)
                .Where(x => x.Id == rssToReadId)
                .Where(x => x.RssChannelSubscription.UserId == currentUserId)
                .Single();

            rssEntryToRead.IsViewed = true;
            rssEntryToRead.IsRead = true;

            this.database.SaveChanges();

        }


    }
}