using AutoMapper;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.Services.Implementation;
using IsThereAnyNews.ViewModels;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssChannelServiceTests
{
    public class RssChannelsService_GetViewModelFormChannelId_Tests
    {
        private AutoMoqer moqer;
        private RssChannelsService sut;
        private Mock<IRssChannelsRepository> mockChannelsRepository;
        private Mock<IMapper> mockMapper;
        private Mock<IUserAuthentication> mockUserAuthentication;

        public RssChannelsService_GetViewModelFormChannelId_Tests()
        {
            this.moqer = new AutoMoq.AutoMoqer();

            this.mockChannelsRepository = this.moqer.GetMock<IRssChannelsRepository>();
            this.mockMapper = this.moqer.GetMock<IMapper>();
            this.mockUserAuthentication = this.moqer.GetMock<IUserAuthentication>();

            this.sut = this.moqer.Resolve<RssChannelsService>();
        }

        [Fact]
        public void T001_When_Creating_RssChannelIndexViewModel_From_Channel_Id_Then_Channel_Must_Be_Read_From_Repository()
        {
            // arrange
            var channelId = 33;

            this.mockChannelsRepository
                .Setup(s => s.LoadRssChannel(It.IsAny<long>()))
                .Returns(new RssChannel());

            // act
            this.sut.GetViewModelFormChannelId(channelId);

            // assert 
            this.mockChannelsRepository.Verify(v => v.LoadRssChannel(It.Is<long>(p => p == 33)), Times.Once());
        }

        [Fact]
        public void T002_When_Repository_Returns_Model_Then_Automapper_Must_Be_Called()
        {
            // arrange
            var channelId = 33;

            this.mockChannelsRepository
                .Setup(s => s.LoadRssChannel(It.IsAny<long>()))
                .Returns(new RssChannel());

            // act
            this.sut.GetViewModelFormChannelId(channelId);

            // assert 
            this.mockMapper
                .Verify(v => v.Map<RssChannelIndexViewModel>(It.IsAny<RssChannel>()), Times.Once());
        }

        [Fact]
        public void T003_When_User_Is_Not_Authenticated_Then_ViewModel_Must_Have_Flag_Set_To_False()
        {
            // arrange
            var channelId = 33;

            this.mockChannelsRepository
                .Setup(s => s.LoadRssChannel(It.IsAny<long>()))
                .Returns(new RssChannel());

            this.mockUserAuthentication
                .Setup(s => s.CurrentUserIsAuthenticated())
                .Returns(false);

            this.mockMapper
                .Setup(s => s.Map<RssChannelIndexViewModel>(It.IsAny<RssChannel>()))
                .Returns(new RssChannelIndexViewModel());

            // act
            var result = this.sut.GetViewModelFormChannelId(channelId);

            // assert 
            Assert.False(result.IsAuthenticatedUser);
        }

        [Fact]
        public void T004_When_User_Is_Authenticated_Then_ViewModel_Must_Have_Flag_Set_To_True_And_UserSubscription_Is_Set()
        {
            // arrange
            var channelId = 33;

            this.mockChannelsRepository
                .Setup(s => s.LoadRssChannel(It.IsAny<long>()))
                .Returns(new RssChannel());

            this.mockUserAuthentication
                .Setup(s => s.CurrentUserIsAuthenticated())
                .Returns(true);

            this.mockMapper
                .Setup(s => s.Map<RssChannelIndexViewModel>(It.IsAny<RssChannel>()))
                .Returns(new RssChannelIndexViewModel());

            this.mockMapper
                .Setup(s => s.Map<UserRssSubscriptionInfoViewModel>(It.IsAny<long>()))
                .Returns(new UserRssSubscriptionInfoViewModel());

            // act
            var result = this.sut.GetViewModelFormChannelId(channelId);

            // assert 
            Assert.True(result.IsAuthenticatedUser);
            Assert.NotNull(result.SubscriptionInfo);
        }
    }
}