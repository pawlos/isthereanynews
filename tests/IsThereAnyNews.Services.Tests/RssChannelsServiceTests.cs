using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests
{
    public class RssChannelsServiceTests
    {
        private Mock<ISessionProvider> sessionProvider;
        private Mock<IRssChannelsRepository> rssChannelsRepository;
        private Mock<IRssChannelsSubscriptionsRepository> rssChannelsSubscriptionsRepository;
        private Mock<IUserRepository> userRepository;
        private Mock<IRssEntriesToReadRepository> rssEntriesToReadRepository;
        private Mock<IUserAuthentication> userAuthentication;
        private AutoMoqer moqer;

        public RssChannelsServiceTests()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.rssChannelsRepository = moqer.GetMock<IRssChannelsRepository>();
            this.sessionProvider = moqer.GetMock<ISessionProvider>();
            this.rssChannelsSubscriptionsRepository = moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.userRepository = moqer.GetMock<IUserRepository>();
            this.rssEntriesToReadRepository = moqer.GetMock<IRssEntriesToReadRepository>();
            this.userAuthentication = moqer.GetMock<IUserAuthentication>();
            this.rssChannelsSubscriptionsRepository = moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.sessionProvider = moqer.GetMock<ISessionProvider>();
        }

        [Fact]
        public void T001()
        {
            var rssChannelsService = this.moqer.Resolve<RssChannelsService>();

            this.rssChannelsRepository.Setup(x => x.LoadAllChannelsWithStatistics())
                .Returns(new List<RssChannelSubscriptionWithStatisticsData>());

            var rssChannelsIndexViewModel = rssChannelsService.LoadAllChannels();

            Assert.Equal(0, rssChannelsIndexViewModel.AllChannels.Count);
        }
    }
}
