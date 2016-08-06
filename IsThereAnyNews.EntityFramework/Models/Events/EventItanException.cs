namespace IsThereAnyNews.EntityFramework.Models.Events
{
    using System;

    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public class EventItanException : IEntity, ICreatable
    {
        public long Id { get; set; }

        public DateTime Created { get; set; }

        public Guid ErrorId { get; set; }

        public long ItanExceptionId { get; set; }
        public ItanException ItanException { get; set; }
    }
}