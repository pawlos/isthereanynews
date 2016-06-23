using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    public class SubscribeCurrentUserToChannel
    {
        private readonly AutoMoqer moqer;
        private readonly RssSubscriptionService sut;
        private readonly Mock<ISessionProvider> mockSessionProvider;
        private readonly Mock<IRssChannelsSubscriptionsRepository> mockRssChannelSubscriptionRepository;

        public SubscribeCurrentUserToChannel()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
            this.mockRssChannelSubscriptionRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
        }

        [Fact]
        public void Subscribing_To_Channel_Must_Load_Current_User_From_Session()
        {
            // act
            this.sut.SubscribeCurrentUserToChannel(0);

            // assert
            this.mockSessionProvider
                .Verify(v => v.GetCurrentUserId(),
                    Times.Once);
        }

        [Fact]
        public void Subscribing_To_Channel_Must_Save_That_Into_Db_Via_Repository()
        {
            // act
            this.sut.SubscribeCurrentUserToChannel(0);

            // assert
            this.mockRssChannelSubscriptionRepository
                .Verify(v => v.CreateNewSubscriptionForUserAndChannel(It.IsAny<long>(), It.IsAny<long>()),
                    Times.Once);
        }
    }
}