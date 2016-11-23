namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;

    using IsThereAnyNews.EntityFramework.Models.Interfaces;
    using IsThereAnyNews.SharedData;

    public class FeatureRequest : IEntity, ICreatable, IModifiable
    {
        public FeatureRequest()
            : this(0, 0, StreamType.Unknown, FeatureRequestType.Unknown)
        {
        }

        public FeatureRequest(
            long userId,
            long rssEntryId,
            StreamType modelStreamType,
            FeatureRequestType featureRequestType)
        {
            this.UserId = userId;
            this.RssEntryId = rssEntryId;
            this.StreamType = modelStreamType;
            this.Type = featureRequestType;
        }

        public long Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public FeatureRequestType Type { get; set; }

        public long UserId { get; set; }

        public long RssEntryId { get; set; }

        public StreamType StreamType { get; set; }

        public User User { get; set; }

        public RssEntry RssEntry { get; set; }
    }
}