//using IsThereAnyNews.Extensions;

//namespace IsThereAnyNews.Services.TestSupport
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Data.Entity;
//    using System.Linq;

//    using Faker;

//    using IsThereAnyNews.DataAccess;
//    using IsThereAnyNews.EntityFramework;
//    using IsThereAnyNews.EntityFramework.Models.Entities;
//    using IsThereAnyNews.ProjectionModels.Mess;
//    using IsThereAnyNews.SharedData;

//    public class TestService : ITestService
//    {
//        private readonly IEntityRepository entityRepository;
//        private readonly ItanDatabaseContext database;

//        public TestService(
//            IEntityRepository entityRepository,
//            ItanDatabaseContext database)
//        {
//            this.entityRepository = entityRepository;
//            this.database = database;
//        }

//        public void GenerateUsers()
//        {
//            this.FixUsersWithEmptyNames();
//            for (int i = 0; i < 1000; i++)
//            {
//                this.entityRepository.CreateNewUser(Faker.Name.FullName(), Faker.Internet.Email());
//            }
//        }

//        private void FixUsersWithEmptyNames()
//        {
//            var emptyDisplay = this.entityRepository.GetAllUsers()
//                .Where(user => string.IsNullOrWhiteSpace(user.DisplayName))
//                .ToList();

//            emptyDisplay.ForEach(user => user.DisplayName = Name.FullName());
//            this.entityRepository.UpdateDisplayNames(emptyDisplay);
//        }

//        public void DuplicateChannels()
//        {
//            var channels = this.entityRepository.LoadAllChannels();
//            var r = new Random(DateTime.Now.Millisecond);
//            var rssSourceWithUrlAndTitles = new List<RssSourceWithUrlAndTitle>(1000);
//            for (int i = 0; i < 1000; i++)
//            {
//                var idx = r.Next(channels.Count);
//                var c = channels[idx];
//                var rssChannel = new RssSourceWithUrlAndTitle(c.Url, c.Title + DateTime.Now.Millisecond);
//                rssSourceWithUrlAndTitles.Add(rssChannel);
//            }
//            this.entityRepository.SaveToDatabase(rssSourceWithUrlAndTitles);
//        }

//        public void CreateSubscriptions()
//        {
//            var users = this.entityRepository.GetAllUsers();
//            var channels = this.entityRepository.LoadAllChannels();
//            var r = new Random(DateTime.Now.Millisecond);

//            for (int i = 0; i < 1000; i++)
//            {
//                var u = users[r.Next(users.Count)];
//                var c = channels[r.Next(channels.Count)];

//                this.entityRepository.Subscribe(c.Id, u.Id, c.Title);
//            }
//        }

//        public void CreateRssToRead()
//        {
//            var users = this.entityRepository.GetAllUsers();
//            foreach (var user in users)
//            {
//                var rssSubscriptions = this.entityRepository.LoadAllSubscriptionsForUser(user.Id);
//                this.entityRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(user.Id, rssSubscriptions);
//            }
//        }

//        public void CreateRssViewedEvent()
//        {
//            for (int i = 0; i < 100; i++)
//            {
//                this.ReadRandomRssEvent();
//            }
//        }

//        public void ReadRandomRssEvent()
//        {
//            var users = this.entityRepository.GetAllUsers();
//            var user = users.Random();
//            var channelSubscriptions = this.entityRepository.LoadAllSubscriptionsForUser(user.Id);
//            this.entityRepository.CopyRssThatWerePublishedAfterLastReadTimeToUser(user.Id, channelSubscriptions);
//            if (!channelSubscriptions.Any())
//            {
//                return;
//            }
//            var subscription = channelSubscriptions.Random();
//            var entryToReads = this.entityRepository.LoadAllUnreadEntriesFromSubscription(subscription.Id);
//            if (!entryToReads.Any())
//            {
//                return;
//            }
//            var entry = entryToReads.Random();
//            this.entityRepository.MarkEntryViewedByUser(user.Id, entry.Id);
//            this.entityRepository.AddEventRssViewed(user.Id, entry.Id);
//        }

//        public void AssignUserRolesToAllUsers()
//        {
//            var users = this.database
//                .Users
//                .Include(x => x.UserRoles)
//                .Where(x => x.UserRoles.Count == 0)
//                .ToList();

//            users.ForEach(x => x.UserRoles.Add(new UserRole() { RoleType = ItanRole.User }));
//            this.database.SaveChanges();
//        }

//        public void FixDuplicatedChannels()
//        {
//            var rssChannels = this.database.RssChannels.ToList();
//            var groupByUrl = rssChannels.GroupBy(x => x.Url);
//            foreach (IGrouping<string, RssChannel> grouping in groupByUrl)
//            {
//                grouping.OrderBy(x => x.Created).Skip(1).ToList().ForEach(x => this.database.RssChannels.Remove(x));
//            }

//            this.database.SaveChanges();
//        }

//        public void FixDuplicatedEntries()
//        {
//            var list = this.database.RssEntries.GroupBy(r => r.Title).Where(g => g.Count() > 1).ToList();
//            foreach (IGrouping<string, RssEntry> grouping in list)
//            {
//                var todelete = grouping.OrderBy(x => x.Created).Skip(1).ToList();
//                foreach (var rssEntry in todelete)
//                {
//                    this.database.RssEntries.Remove(rssEntry);
//                }
//                this.database.SaveChangesAsync();
//            }
//        }

//        public void FixSubscriptions()
//        {
//            List<RssChannelSubscription> rssChannelSubscriptions = this.database.RssChannelsSubscriptions.ToList();
//            List<IGrouping<long, RssChannelSubscription>> groupByUsers =
//                rssChannelSubscriptions.GroupBy(x => x.UserId).ToList();

//            foreach (IGrouping<long, RssChannelSubscription> userSubscription in groupByUsers)
//            {
//                var withDuplicate = userSubscription.GroupBy(x => x.RssChannelId).Where(g => g.Count() > 1);
//                {
//                    foreach (var duplicate in withDuplicate)
//                    {
//                        duplicate.Skip(1).ToList().ForEach(x => this.database.RssChannelsSubscriptions.Remove(x));
//                        if (duplicate.Count() > 1)
//                        {
//                            this.database.SaveChanges();
//                        }
//                    }
//                }
//            }
//        }
//    }
//}