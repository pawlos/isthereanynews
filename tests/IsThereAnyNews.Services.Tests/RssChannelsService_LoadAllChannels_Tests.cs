using System.Collections.Generic;
using AutoMapper;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.Services.Implementation;
using IsThereAnyNews.ViewModels;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests
{
    public class RssChannelsService_LoadAllChannels_Tests
    {
        private readonly AutoMoqer moqer;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRssChannelsRepository> mockRssChannelsRepository;
        private readonly IRssChannelsService sut;

        public RssChannelsService_LoadAllChannels_Tests()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            mockRssChannelsRepository = moqer.GetMock<IRssChannelsRepository>();
            this.mockMapper = moqer.GetMock<IMapper>();
            this.sut = this.moqer.Resolve<RssChannelsService>();
        }

        [Fact]
        public void T001_When_Loading_AllChannels_Then_LoadFromRepositoryIsCalled()
        {
            // arrange 

            this.mockRssChannelsRepository.Setup(x => x.LoadAllChannelsWithStatistics())
                .Returns(new List<RssChannelSubscriptionWithStatisticsData>
                {
                    new RssChannelSubscriptionWithStatisticsData(),
                    new RssChannelSubscriptionWithStatisticsData(),
                    new RssChannelSubscriptionWithStatisticsData()
                });

            // act
            var rssChannelsIndexViewModel = sut.LoadAllChannels();

            // assert
            this.mockRssChannelsRepository.Verify(x => x.LoadAllChannelsWithStatistics(), Times.Once());
        }

        [Fact]
        public void T002_When_Repository_Returned_Values_Then_Automapper_Is_Called()
        {
            // arrange 
            this.mockRssChannelsRepository.Setup(x => x.LoadAllChannelsWithStatistics())
                .Returns(new List<RssChannelSubscriptionWithStatisticsData>
                {
                    new RssChannelSubscriptionWithStatisticsData(),
                    new RssChannelSubscriptionWithStatisticsData(),
                    new RssChannelSubscriptionWithStatisticsData()
                });

            // act
            var rssChannelsIndexViewModel = sut.LoadAllChannels();

            // assert
            this.mockMapper.Verify(
                x => x.Map<RssChannelsIndexViewModel>(It.IsAny<List<RssChannelSubscriptionWithStatisticsData>>()),
                Times.Once());
        }
    }
}
