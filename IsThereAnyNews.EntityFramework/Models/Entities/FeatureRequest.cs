using System;
using System.Data.SqlTypes;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.EntityFramework.Models.Interfaces;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.EntityFramework.Models
{
    public class FeatureRequest : IEntity, ICreatable, IModifiable
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public FeatureRequestType Type { get; set; }
        public long UserId { get; set; }
        public long RssEntryId { get; set; }

        public User User { get; set; }
        public RssEntry RssEntry { get; set; }

    }
}