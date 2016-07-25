namespace IsThereAnyNews.EntityFramework.Models.Events
{
    using System;

    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public class ContactAdministrationEvent : IEntity, ICreatable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }

        public long ContactAdministrationId { get; set; }
        public ContactAdministration ContactAdministration { get; set; }
    }
}