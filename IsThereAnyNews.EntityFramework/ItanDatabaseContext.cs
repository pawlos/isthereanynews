using System;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.EntityFramework.Models.Events;
using IsThereAnyNews.EntityFramework.Models.Interfaces;

namespace IsThereAnyNews.EntityFramework
{
    public class ItanDatabaseContext : DbContext
    {
        public ItanDatabaseContext() : base("itan-database")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.Log = text => System.Diagnostics.Debug.WriteLine(text);
        }

        public override int SaveChanges()
        {
            var now = DateTime.Now;
            MakeCreationDateStamp(now);
            UpdateEditionDateStamp(now);
            return base.SaveChanges();
        }

        private void UpdateEditionDateStamp(DateTime now)
        {
            var selectedEntityList = ChangeTracker.Entries()
                            .Where(x => x.Entity is IModifiable && x.State == EntityState.Modified)
                            .Select(e => e.Entity)
                            .Cast<IModifiable>()
                            .ToList();

            selectedEntityList.ForEach(model =>
            {
                model.Updated = now;
            });
        }

        private void MakeCreationDateStamp(DateTime now)
        {
            var selectedEntityList = ChangeTracker.Entries()
                .Where(x => x.Entity is ICreatable && x.State == EntityState.Added)
                .Select(e => e.Entity)
                .Cast<ICreatable>()
                .ToList();

            selectedEntityList.ForEach(model =>
            {
                model.Created = now;
                var m = model as IModifiable;
                if (m != null)
                {
                    m.Updated = now;
                }
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RssChannel> RssChannels { get; set; }
        public DbSet<RssChannelSubscription> RssChannelsSubscriptions { get; set; }
        public DbSet<SocialLogin> SocialLogins { get; set; }
        public DbSet<RssEntry> RssEntries { get; set; }
        public DbSet<RssEntryToRead> RssEntriesToRead { get; set; }
        public DbSet<FeatureRequest> FeatureRequests { get; set; }
        public DbSet<UserSubscription> UsersSubscriptions { get; set; }
        public DbSet<UserSubscriptionEntryToRead> UsersSubscriptionsToRead { get; set; }

        public DbSet<EventRssViewed> EventsRssViewed { get; set; }
        public DbSet<EventRssChannelUpdated> RssChannelUpdates { get; set; }
        public DbSet<EventRssClicked> EventsRssClicked { get; set; }

    }
}