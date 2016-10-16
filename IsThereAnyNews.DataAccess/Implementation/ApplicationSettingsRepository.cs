namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Linq;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.SharedData;

    public class ApplicationSettingsRepository : IApplicationSettingsRepository
    {
        private readonly ItanDatabaseContext database;

        public ApplicationSettingsRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public RegistrationSupported GetCurrentRegistrationStatus()
        {
            return this.database.ApplicationConfiguration.Single().RegistrationSupported;
        }

        public bool CanRegisterWithinLimits()
        {
            var applicationConfiguration = this.database.ApplicationConfiguration.Single();
            return applicationConfiguration.UsersLimit > this.database.Users.Count();
        }
    }
}