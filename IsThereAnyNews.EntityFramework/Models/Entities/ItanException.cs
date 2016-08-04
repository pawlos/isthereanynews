namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;

    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public class ItanException : IEntity, ICreatable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Typeof { get; set; }
        public string Stacktrace { get; set; }
        public Guid ErrorId { get; set; }
    }
}