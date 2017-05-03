using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.Automapper;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.Infrastructure.Import.Opml;
using IsThereAnyNews.Services;

using System.Data.Entity;
using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.ContentGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var xxxxxx = 1000;

            ItanDatabaseContext database = new ItanDatabaseContext();
            database.Configuration.ValidateOnSaveEnabled = false;

            //database.Database.Log += s => Console.WriteLine($"***{s}***");
            var configureMapper = IsThereAnyNewsAutomapper.ConfigureMapper();

            var itanDatabaseContext = new ItanDatabaseContext();
            var updateRepository = new EntityRepository(itanDatabaseContext, configureMapper);
            var infrastructure = new Infrastructure.Web.Infrastructure();
            var importOpml = new ImportOpml();

            var service = new Service(updateRepository, configureMapper, null, infrastructure, importOpml);
            List<User> users = new List<User>();

            Console.WriteLine($"creating {xxxxxx} users ");
            for(int i = 0; i < xxxxxx; i++)
            {
                Console.WriteLine($"{i:D4} of {xxxxxx}");
                var entity = new User { DisplayName = Faker.NameFaker.MaleName(), Email = Faker.InternetFaker.Email() };
                users.Add(entity);
                database.Users.Add(entity);
            }

            database.SaveChanges();

            var rssChannels = database.RssChannels.Include(i => i.RssEntries).ToList();

            var rssChannelsSubscriptions = new List<RssChannelSubscription>();
            Console.WriteLine("creating subscriptions");
            for(int i = 0; i < xxxxxx; i++)
            {
                Console.WriteLine($"{i:D4} of {xxxxxx}");
                for (var index = 0; index < rssChannels.Count; index++)
                {
                    var rssChannel = rssChannels[index];
                    Console.WriteLine($"{i:D4}\t{index:D5} of {rssChannels.Count}");
                    var rssChannelSubscription = new RssChannelSubscription(rssChannel.Id, users[i].Id, rssChannel.Title);
                    rssChannelsSubscriptions.Add(rssChannelSubscription);
                }
            }

            database.RssChannelsSubscriptions.AddRange(rssChannelsSubscriptions);
            database.SaveChanges();


            var rssEntriesToRead = new List<RssEntryToRead>();
            var eventRssUserInteractions = new List<EventRssUserInteraction>();
            for (int i = 0; i < xxxxxx; i++)
            {
                for (var index = 0; index < rssChannels.Count; index++)
                {
                    var rssChannel = rssChannels[index];
                    for (var rssEntryIndex = 0; rssEntryIndex < rssChannel.RssEntries.Count; rssEntryIndex++)
                    {
                        Console.WriteLine($"{i:d5} of {xxxxxx}\t channel:{index:d5} of {rssChannels.Count}\t{rssEntryIndex:d5} of {rssChannel.RssEntries.Count}");
                        var rssChannelRssEntry = rssChannel.RssEntries[rssEntryIndex];
                        var rssEntryToRead = new RssEntryToRead
                        {
                            IsRead = Faker.BooleanFaker.Boolean(),
                            IsViewed = Faker.BooleanFaker.Boolean(),
                            RssEntryId = rssChannelRssEntry.Id,
                            RssChannelSubscriptionId =
                                rssChannelsSubscriptions.Single(
                                    x => x.UserId == users[i].Id && x.RssChannelId == rssChannel.Id).Id,
                        };
                        rssEntriesToRead.Add(rssEntryToRead);
                        var eventRssUserInteraction = new EventRssUserInteraction
                        {
                            InteractionType = Faker.EnumFaker.SelectFrom<InteractionType>(),
                            RssEntryId = rssChannelRssEntry.Id,
                            UserId = users[i].Id
                        };
                        eventRssUserInteractions.Add(eventRssUserInteraction);
                    }
                }

                database.RssEntriesToRead.AddRange(rssEntriesToRead);
                database.SaveChanges();

                database.EventsRssUserInteraction.AddRange(eventRssUserInteractions);
                database.SaveChanges();

                rssEntriesToRead.Clear();
                eventRssUserInteractions.Clear();

            }

            
        }
    }
}
