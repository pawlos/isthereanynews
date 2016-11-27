namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;

    using IsThereAnyNews.EntityFramework.Models.Interfaces;
    using IsThereAnyNews.SharedData;

    public class UserRole : IEntity, ICreatable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public ItanRole RoleType { get; set; }
    }
}