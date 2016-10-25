namespace IsThereAnyNews.EntityFramework.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<IsThereAnyNews.EntityFramework.ItanDatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        //protected override void Seed(ItanDatabaseContext context)
        //{
        //    context.ApplicationConfiguration.Add(
        //        new ApplicationConfiguration { RegistrationSupported = RegistrationSupported.Open, UsersLimit = 1 });
        //    context.SaveChanges();
        //    base.Seed(context);
        //}
    }
}
