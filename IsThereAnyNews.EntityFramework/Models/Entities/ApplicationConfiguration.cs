namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;

    using IsThereAnyNews.EntityFramework.Models.Interfaces;
    using IsThereAnyNews.SharedData;

    public class ApplicationConfiguration : IEntity, ICreatable, IModifiable
    {
        public long Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public RegistrationSupported RegistrationSupported { get; set; }

        public long UsersLimit { get; set; }
    }
}