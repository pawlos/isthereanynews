using System;
using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models;
using Faker;

namespace IsThereAnyNews.Services.Implementation
{
    public class TestService : ITestService
    {
        private readonly IUserRepository usersRepository;

        private readonly IRssChannelsRepository rssChannelsRepository;

        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionRepository;
        private readonly IRssEntriesToReadRepository rssToReadRepository;

        public TestService(IUserRepository usersRepository, IRssChannelsRepository rssChannelsRepository, IRssChannelsSubscriptionsRepository rssSubscriptionRepository, IRssEntriesToReadRepository rssToReadRepository)
        {
            this.usersRepository = usersRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.rssSubscriptionRepository = rssSubscriptionRepository;
            this.rssToReadRepository = rssToReadRepository;
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
    }
}