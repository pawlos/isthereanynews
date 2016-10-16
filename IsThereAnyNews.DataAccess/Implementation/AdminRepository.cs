namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Linq;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class AdminRepository : IAdminRepository
    {
        private readonly ItanDatabaseContext database;

        public AdminRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public ApplicationConfiguration LoadApplicationConfiguration()
        {
            return this.database.ApplicationConfiguration.Single();
        }

        public long GetNumberOfRegisteredUsers()
        {
            return this.database.Users.Count();
        }

        public long GetNumberOfRssSubscriptions()
        {
            return this.database.RssChannelsSubscriptions.Count();
        }

        public long GetNumberOfRssNews()
        {
            return this.database.RssEntries.Count();
        }
    }
}