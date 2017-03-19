namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.SharedData;

    internal sealed class Configuration : DbMigrationsConfiguration<ItanDatabaseContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ItanDatabaseContext context)
        {
            var count = context.ApplicationConfiguration.Count();
            if (count == 0)
            {
                context.ApplicationConfiguration.Add(new ApplicationConfiguration
                {
                    Created = DateTime.Now,
                    RegistrationSupported = RegistrationSupported.Open,
                    UsersLimit = 1
                });
                context.SaveChanges();
            }
        }
    }
}