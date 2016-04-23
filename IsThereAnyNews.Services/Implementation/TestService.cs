using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models;
using Faker;
using Faker.Extensions;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.Extensions;

namespace IsThereAnyNews.Services.Implementation
{
    public class TestService : ITestService
    {
        private readonly IUserRepository usersRepository;

        private readonly IRssChannelsRepository rssChannelsRepository;

        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionRepository;
        private readonly IRssEntriesToReadRepository rssToReadRepository;
        private readonly IRssEventRepository rssEventRepository;
        private readonly ItanDatabaseContext database;

        public TestService(
            IUserRepository usersRepository,
            IRssChannelsRepository rssChannelsRepository,
            IRssChannelsSubscriptionsRepository rssSubscriptionRepository,
            IRssEntriesToReadRepository rssToReadRepository,
            IRssEventRepository rssEventRepository,
            ItanDatabaseContext database)
        {
            this.usersRepository = usersRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.rssSubscriptionRepository = rssSubscriptionRepository;
            this.rssToReadRepository = rssToReadRepository;
            this.rssEventRepository = rssEventRepository;
            this.database = database;
        }

        public void GenerateUsers()
        {
            FixUsersWithEmptyNames();
            for (int i = 0; i < 1000; i++)
            {
                this.usersRepository.CreateNewUser();
            }
        }

        private void FixUsersWithEmptyNames()
        {
            var emptyDisplay = this.usersRepository.GetAllUsers()
                .Where(user => string.IsNullOrWhiteSpace(user.DisplayName))
                .ToList();

            emptyDisplay.ForEach(user => user.DisplayName = Name.FullName());
            this.usersRepository.UpdateDisplayNames(emptyDisplay);
        }

        public void DuplicateChannels()
        {
            var channels = this.rssChannelsRepository.LoadAllChannels();
            var r = new Random(DateTime.Now.Millisecond);
            var duplicates = new List<RssChannel>();
            for (int i = 0; i < 1000; i++)
            {
                var idx = r.Next(channels.Count);
                var c = channels[idx];
                duplicates.Add(new RssChannel { Title = c.Title + DateTime.Now.Millisecond, Url = c.Url });
            }

            this.rssChannelsRepository.SaveToDatabase(duplicates);
        }

        public void CreateSubscriptions()
        {
            var users = this.usersRepository.GetAllUsers();
            var channels = this.rssChannelsRepository.LoadAllChannels();
            var r = new Random(DateTime.Now.Millisecond);

            var subscriptions = new List<RssChannelSubscription>();

            for (int i = 0; i < 1000; i++)
            {
                var u = users[r.Next(users.Count)];
                var c = channels[r.Next(channels.Count)];

                subscriptions.Add(new RssChannelSubscription(c.Id, u.Id, c.Title));
            }

            this.rssSubscriptionRepository.SaveToDatabase(subscriptions);
        }

        public void CreateRssToRead()
        {
            var r = new Random(DateTime.Now.Millisecond);
            var users = this.usersRepository.GetAllUsers();
            var user = users[r.Next(users.Count)];

            var rssSubscriptions = this.rssSubscriptionRepository.LoadAllSubscriptionsForUser(user.Id);
            this.rssToReadRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(user.Id, rssSubscriptions);
        }

        public void CreateRssViewedEvent()
        {
            var users = this.usersRepository.GetAllUsers();
            for (int i = 0; i < 100; i++)
            {
                var user = users.Random();
                var channelSubscriptions = this.rssSubscriptionRepository.LoadAllSubscriptionsForUser(user.Id);
                this.rssToReadRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(user.Id, channelSubscriptions);
                if (!channelSubscriptions.Any())
                {
                    continue;
                }
                var subscription = channelSubscriptions.Random();
                var entryToReads = rssToReadRepository.LoadAllUnreadEntriesFromSubscription(subscription.Id);
                if (!entryToReads.Any())
                {
                    continue;
                }
                var entry = entryToReads.Random();
                rssToReadRepository.MarkEntryViewedByUser(user.Id, entry.Id);
                rssEventRepository.AddEventRssViewed(user.Id, entry.Id);
            }
        }

        public void FixSubscriptions()
        {
            List<RssChannelSubscription> rssChannelSubscriptions = this.database.RssChannelsSubscriptions.ToList();
            List<IGrouping<long, RssChannelSubscription>> groupByUsers =
                rssChannelSubscriptions.GroupBy(x => x.UserId).ToList();

            foreach (IGrouping<long, RssChannelSubscription> userSubscription in groupByUsers)
            {
                var withDuplicate = userSubscription.GroupBy(x => x.RssChannelId).Where(g => g.Count() > 1);
                {
                    foreach (var duplicate in withDuplicate)
                    {
                        duplicate.Skip(1).ToList().ForEach(x => this.database.RssChannelsSubscriptions.Remove(x));
                        if (duplicate.Count() > 1)
                        {
                            this.database.SaveChanges();
                        }
                    }
                }
            }

        }
    }
}