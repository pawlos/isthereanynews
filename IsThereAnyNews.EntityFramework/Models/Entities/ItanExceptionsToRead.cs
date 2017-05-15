namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;
    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public class ItanExceptionToRead: IEntity, ICreatable, IModifiable
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsViewed { get; set; }
        public bool IsSkipped { get; set; }

        public long ItanExceptionId { get; set; }
        public ItanException ItanException { get; set; }
    }
}