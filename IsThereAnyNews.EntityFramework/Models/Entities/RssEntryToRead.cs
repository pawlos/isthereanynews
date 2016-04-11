using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using IsThereAnyNews.EntityFramework.Models.Interfaces;

namespace IsThereAnyNews.EntityFramework.Models
{
    [Table("RssEntriesToRead")]
    public sealed class RssEntryToRead : IEntity, ICreatable, IModifiable
    {
        public RssEntryToRead()
        { }

        public RssEntryToRead(RssEntry rssEntry, long rssChannelSubscriptionId)
        {
            this.RssEntryId = rssEntry.Id;
            this.RssEntry = rssEntry;
            this.RssChannelSubscriptionId = rssChannelSubscriptionId;
        }

        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public bool IsRead { get; set; }
        public bool IsViewed { get; set; }

        public long RssChannelSubscriptionId { get; set; }
        public long RssEntryId { get; set; }

        public RssChannelSubscription RssChannelSubscription { get; set; }
        [Required]
        public RssEntry RssEntry { get; set; }
    }
}