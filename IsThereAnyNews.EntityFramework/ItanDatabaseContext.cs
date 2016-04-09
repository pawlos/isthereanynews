using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using IsThereAnyNews.EntityFramework.Models;

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

            var selectedEntityList = ChangeTracker.Entries()
                .Where(x => x.Entity is IModel &&
                            (x.State == EntityState.Added || x.State == EntityState.Modified))
                .Select(e => e.Entity)
                .Cast<IModel>()
                .ToList();

            selectedEntityList.ForEach(model =>
            {
                if (model.Created == default(DateTime))
                {
                    model.Created = now;
                }

                model.Updated = now;
            });

            return base.SaveChanges();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RssChannel> RssChannels { get; set; }
        public DbSet<RssChannelSubscription> RssChannelsSubscriptions { get; set; }
        public DbSet<SocialLogin> SocialLogins { get; set; }
        public DbSet<RssEntry> RssEntries { get; set; }
        public DbSet<RssEntryToRead> RssEntriesToRead { get; set; }
        public DbSet<FeatureRequest> FeatureRequests { get; set; }
    }
}