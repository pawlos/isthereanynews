using System.Data.Entity;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.EntityFramework
{
    public class ItanDatabaseContext : DbContext
    {
        public ItanDatabaseContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.Log = text => System.Diagnostics.Debug.WriteLine(text);
        }
        public DbSet<ItanUser> Users { get; private set; }
        public DbSet<RssChannel> RssChannels { get; private set; }
    }
}