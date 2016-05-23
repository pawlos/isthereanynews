using AutoMapper;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Services.Implementation;
using IsThereAnyNews.ViewModels;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssChannelService
{
    public class RssChannelsService_CreateUserSubscriptionInfo_Tests
    {
        private AutoMoqer moqer;
        private RssChannelsService sut;
        private Mock<ISessionProvider> mockSessionProvider;
        private Mock<IRssChannelsSubscriptionsRepository> mockRssSubscriptionRepository;
        private Mock<IMapper> mockMapper;

        public RssChannelsService_CreateUserSubscriptionInfo_Tests()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssChannelsService>();
            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
            this.mockRssSubscriptionRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.mockMapper = this.moqer.GetMock<IMapper>();
        }

        [Fact]
        public void T001_When_Subscription_Is_Created_Then_Current_User_Id_Is_Loaded_From_Session()
        {
            // arrange
            var rssChannelId = 45;

            // act
            this.sut.CreateUserSubscriptionInfo(rssChannelId);

            // assert
            this.mockSessionProvider.Verify(v => v.GetCurrentUserId(), Times.Once());
        }

        [Fact]
        public void T002_When_Subscription_Is_Created_Then_Subscription_Id_Is_Loaded_From_Repository()
        {
            // arrange
            var rssChannelId = 45;
            var userId = 25;

            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(userId);

            // act
            this.sut.CreateUserSubscriptionInfo(rssChannelId);

            // assert
            this.mockRssSubscriptionRepository
                .Verify(v => v.FindSubscriptionIdOfUserAndOfChannel(
                    It.Is<long>(p => p == 25),
                    It.Is<long>(p => p == 45)),
                Times.Once());
        }

        [Fact]
        public void T003_When_Repository_Find_Subscription_Then_Automapper_Must_Be_Called()
        {
            // arrange
            var rssChannelId = 45;
            var userId = 25;
            var subscriptionId = 98;

            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(userId);

            this.mockRssSubscriptionRepository
                .Setup(v => v.FindSubscriptionIdOfUserAndOfChannel(
                    It.IsAny<long>(),
                    It.IsAny<long>()))
                .Returns(subscriptionId);

            // act
            this.sut.CreateUserSubscriptionInfo(rssChannelId);

            // assert
            this.mockMapper.Verify(v => v.Map<UserRssSubscriptionInfoViewModel>(It.IsAny<long>()), Times.Once());
        }
    }
}