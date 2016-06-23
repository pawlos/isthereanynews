using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    public class UnsubscribeCurrentUserFromChannelId
    {
        private readonly AutoMoqer moqer;
        private readonly RssSubscriptionService sut;
        private readonly Mock<ISessionProvider> mockSessionProvider;
        private readonly Mock<IRssChannelsSubscriptionsRepository> mockRssChannelSubscriptionRepository;

        public UnsubscribeCurrentUserFromChannelId()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockRssChannelSubscriptionRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
        }

        [Fact]
        public void Unsubscribing_Must_Load_Current_User_From_Session()
        {
            // act
            this.sut.UnsubscribeCurrentUserFromChannelId(0);

            // assert
            this.mockSessionProvider
                .Verify(v => v.GetCurrentUserId(),
                    Times.Once);
        }

        [Fact]
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