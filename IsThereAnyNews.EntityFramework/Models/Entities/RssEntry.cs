﻿namespace IsThereAnyNews.EntityFramework.Models.Entities
{
    using System;
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Interfaces;

    public class RssEntry : IEntity, ICreatable, IModifiable
    {
        public RssEntry() : this(string.Empty, DateTimeOffset.MinValue, string.Empty, string.Empty, string.Empty, 0, string.Empty)
        {
        }

        public RssEntry(string id, DateTimeOffset publishDate, string title, string text, string strippedText, long channelId, string url)
        {
            this.RssId = id;
            this.PublicationDate = publishDate.DateTime;
            this.Title = title;
            this.PreviewText = text;
            this.StrippedText = strippedText;
            this.RssChannelId = channelId;
            this.Url = url;
        }


        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public long RssChannelId { get; set; }
        public RssChannel RssChannel { get; set; }

        public string Url { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Title { get; set; }
        public string PreviewText { get; set; }
        public string StrippedText { get; set; }
        public string RssId { get; set; }

        public List<RssEntryToRead> RssEntryToReads { get; set; }
    }
}