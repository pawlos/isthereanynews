namespace IsThereAnyNews.Automapper
{
    using System;
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ViewModels.RssChannel;

    public class UpdateResolver : IValueResolver<RssChannel, RssChannelIndexViewModel, DateTime>
    {
        public DateTime Resolve(
            RssChannel source,
            RssChannelIndexViewModel destination,
            DateTime destMember,
            ResolutionContext context)
        {
            return source.Updates.OrderByDescending(o => o.Created)
                         .FirstOrDefault()
                         ?.Created ?? DateTime.MinValue;
        }
    }
}