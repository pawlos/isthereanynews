namespace IsThereAnyNews.Services
{
    using System;
    using System.Collections.Generic;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models;

    public interface ITestService
    {
        void GenerateUsers();

        void DuplicateChannels();

        void CreateSubscriptions();
    }

    public class TestService : ITestService
    {
        private readonly IUserRepository usersRepository;

        private readonly IRssChannelsRepository rssChannelsRepository;

        private readonly IRssChannelsSubscriptionsRepository rssSubscriptionRepository;

        public TestService(IUserRepository usersRepository, IRssChannelsRepository rssChannelsRepository, IRssChannelsSubscriptionsRepository rssSubscriptionRepository)
        {
            this.usersRepository = usersRepository;
            this.rssChannelsRepository = rssChannelsRepository;
            this.rssSubscriptionRepository = rssSubscriptionRepository;
        }

        public void GenerateUsers()
        {
            for (int i = 0; i < 1000; i++)
            {
                this.usersRepository.CreateNewUser();
            }
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
    }
}
