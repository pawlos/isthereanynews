namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.SharedData;

    public class AdminRepository : IAdminRepository
    {
        private readonly ItanDatabaseContext database;

        private readonly IMapper mapper;

        public AdminRepository(ItanDatabaseContext database, IMapper mapper)
        {
            this.database = database;
            this.mapper = mapper;
        }

        public ApplicationConfigurationDTO LoadApplicationConfiguration()
        {
            var applicationConfiguration = this.database.ApplicationConfiguration.Single();
            var dto = this.mapper.Map<ApplicationConfiguration, ApplicationConfigurationDTO>(applicationConfiguration);
            return dto;
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

        public void ChangeApplicationRegistration(RegistrationSupported dtoStatus)
        {
            this.database.ApplicationConfiguration.Single().RegistrationSupported = dtoStatus;
            this.database.SaveChanges();
        }

        public void ChangeUserLimit(long dtoLimit)
        {
            this.database.ApplicationConfiguration.Single().UsersLimit = dtoLimit;
            this.database.SaveChanges();
        }
    }
}