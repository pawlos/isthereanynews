namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Services.Implementation;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class UnsubscribeCurrentUserFromChannelId
    {
        private AutoMoqer moqer;
        private RssSubscriptionService sut;
        private Mock<ISessionProvider> mockSessionProvider;
        private Mock<IRssChannelsSubscriptionsRepository> mockRssChannelSubscriptionRepository;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockRssChannelSubscriptionRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
        }

        [Test]
        public void Unsubscribing_Must_Load_Current_User_From_Session()
        {
            // act
            this.sut.UnsubscribeCurrentUserFromChannelId(0);

            // assert
            this.mockSessionProvider
                .Verify(v => v.GetCurrentUserId(),
                    Times.Once);
        }

        [Test]
        public void Unsubscribing_Must_Delete_Subscription_From_User_Via_Repository()
        {
            // act
            this.sut.UnsubscribeCurrentUserFromChannelId(0);

            // assert
            this.mockRssChannelSubscriptionRepository
                .Verify(v => v.DeleteSubscriptionFromUser(It.IsAny<long>(), It.IsAny<long>()),
                    Times.Once);
        }
    }
}