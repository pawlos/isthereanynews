namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    [Table("RssEntriesToRead")]
    public sealed class RssEntryToRead : IEntity, ICreatable, IModifiable
    {
        public RssEntryToRead()
        {
        }

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
        public bool IsSkipped { get; set; }

        public long RssChannelSubscriptionId { get; set; }
        
        public RssChannelSubscription RssChannelSubscription { get; set; }
        
        public RssEntry RssEntry { get; set; }
        public long RssEntryId { get; set; }
    }
}