namespace IsThereAnyNews.Services.Tests.RssChannelServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.Services.Implementation;
    using IsThereAnyNews.ViewModels;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class LoadAllChannelsOfCurrentUser
    {
        private AutoMoqer moqer;
        private Mock<ISessionProvider> mockSessionProvider;
        private Mock<IRssChannelsSubscriptionsRepository> mockChannelsSubscriptionRepository;
        private Mock<IRssEntriesToReadRepository> mockRssEntriesRepository;
        private IRssChannelsService sut;
        private Mock<IMapper> mockMapper;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();

            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
            this.mockChannelsSubscriptionRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.mockRssEntriesRepository = this.moqer.GetMock<IRssEntriesToReadRepository>();
            this.mockMapper = this.moqer.GetMock<IMapper>();

            this.sut = this.moqer.Resolve<RssChannelsService>();
        }

        [Test]
        public void T001_When_Loading_Channels_Of_Current_User_Then_User_Must_Be_Read_From_Session_Provider()
        {
            // arrange

            // act
            this.sut.LoadAllChannelsOfCurrentUser();

            // assert
            this.mockSessionProvider.Verify(x => x.GetCurrentUserId(), Times.Once());
        }

        [Test]
        public void T002_When_Loading_Channels_Of_Current_User_Then_Repository_Must_Be_Called_With_UserId_Fetched_From_SessionProvider()
        {
            // arrange
            var userIdToUse = 332L;
            this.mockSessionProvider.Setup(s => s.GetCurrentUserId()).Returns(userIdToUse);

            // act
            this.sut.LoadAllChannelsOfCurrentUser();

            // assert
            this.mockSessionProvider.Verify(x => x.GetCurrentUserId(), Times.Once());
            this.mockChannelsSubscriptionRepository
                .Verify(v => v.LoadAllSubscriptionsForUser(It.Is<long>(i => i == 332L)),
                Times.Once());
        }

        [Test]
        public void T003_When_Loading_Channels_Of_Current_Users_Then_RssEntriesToReadRepository_For_Copy_New_Entries()
        {
            // arrange
            var userIdToUse = 332L;
            var subscriptions = new List<RssChannelSubscription>
            {
                new RssChannelSubscription {Id = 333}
            };

            this.mockSessionProvider.Setup(s => s.GetCurrentUserId()).Returns(userIdToUse);
            this.mockChannelsSubscriptionRepository
                .Setup(s => s.LoadAllSubscriptionsForUser(It.IsAny<long>()))
                .Returns(subscriptions);

            // act
            this.sut.LoadAllChannelsOfCurrentUser();

            // assert
            this.mockRssEntriesRepository
                .Verify(v => v.CopyRssThatWerePublishedAfterLastReadTimeToUser(
                    It.Is<long>(p => p == 332),
                    It.Is<List<RssChannelSubscription>>(p => p.Count == 1 && p.Single().Id == 333))
                    , Times.Once());
        }

        [Test]
        public void T004_When_Loading_Channels_Of_Current_Users_Then_Mapper_Must_Be_Called()
        {
            // arrange
            var userIdToUse = 332L;
            var subscriptions = new List<RssChannelSubscription>
            {
                new RssChannelSubscription {Id = 333}
            };

            this.mockSessionProvider.Setup(s => s.GetCurrentUserId()).Returns(userIdToUse);
            this.mockChannelsSubscriptionRepository
                .Setup(s => s.LoadAllSubscriptionsForUser(It.IsAny<long>()))
                .Returns(subscriptions);

            // act
            this.sut.LoadAllChannelsOfCurrentUser();

            // assert
            this.mockMapper
                .Verify(v => v.Map<RssChannelsMyViewModel>(It.IsAny<List<RssChannelSubscription>>()),
                Times.Once());
        }
    }
}