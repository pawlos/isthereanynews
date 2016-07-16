namespace IsThereAnyNews.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Faker;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.Extensions;
    using IsThereAnyNews.SharedData;

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
            for (int i = 0; i < 1000; i++)
            {
                var idx = r.Next(channels.Count);
                var c = channels[idx];
                var rssChannel = new RssChannel { Title = c.Title + DateTime.Now.Millisecond, Url = c.Url };
                this.rssChannelsRepository.SaveToDatabase(new List<RssChannel> { rssChannel });
            }
        }

        public void CreateSubscriptions()
        {
            var users = this.usersRepository.GetAllUsers();
            var channels = this.rssChannelsRepository.LoadAllChannels();
            var r = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < 1000; i++)
            {
                var u = users[r.Next(users.Count)];
                var c = channels[r.Next(channels.Count)];

                var rssChannelSubscription = new RssChannelSubscription(c.Id, u.Id, c.Title);
                this.rssSubscriptionRepository.SaveToDatabase(new List<RssChannelSubscription> { rssChannelSubscription });
            }
        }

        public void CreateRssToRead()
        {
            for (int i = 0; i < 1000; i++)
            {
                var r = new Random(DateTime.Now.Millisecond);
                var users = this.usersRepository.GetAllUsers();
                var user = users[r.Next(users.Count)];

                var rssSubscriptions = this.rssSubscriptionRepository.LoadAllSubscriptionsForUser(user.Id);
                this.rssToReadRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(user.Id, rssSubscriptions);
            }
        }

        public void CreateRssViewedEvent()
        {
            for (int i = 0; i < 100; i++)
            {
                ReadRandomRssEvent();
            }
        }

        public void ReadRandomRssEvent()
        {
            var users = this.usersRepository.GetAllUsers();
            var user = users.Random();
            var channelSubscriptions = this.rssSubscriptionRepository.LoadAllSubscriptionsForUser(user.Id);
            this.rssToReadRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(user.Id, channelSubscriptions);
            if (!channelSubscriptions.Any())
            {
                return;
            }
            var subscription = channelSubscriptions.Random();
            var entryToReads = rssToReadRepository.LoadAllUnreadEntriesFromSubscription(subscription.Id);
            if (!entryToReads.Any())
            {
                return;
            }
            var entry = entryToReads.Random();
            rssToReadRepository.MarkEntryViewedByUser(user.Id, entry.Id);
            rssEventRepository.AddEventRssViewed(user.Id, entry.Id);
        }

        public void AssignUserRolesToAllUsers()
        {
            var users = this.database
                .Users
                .Include(x => x.UserRoles)
                .Where(x => x.UserRoles.Count == 0)
                .ToList();

            users.ForEach(x => x.UserRoles.Add(new UserRole() { RoleType = ItanRole.User }));
            this.database.SaveChanges();
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