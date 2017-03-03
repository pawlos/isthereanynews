using IsThereAnyNews.ViewModels.RssChannel;

namespace IsThereAnyNews.Services.Tests.RssChannelServiceTests
{
    using System.Collections.Generic;

    using AutoMapper;

    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.Services.Implementation;
    using IsThereAnyNews.ViewModels;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class LoadAllChannels
    {
        private AutoMoqer moqer;
        private Mock<IMapper> mockMapper;
        private Mock<IRssChannelsRepository> mockRssChannelsRepository;
        private IRssChannelsService sut;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            mockRssChannelsRepository = moqer.GetMock<IRssChannelsRepository>();
            this.mockMapper = moqer.GetMock<IMapper>();
            this.sut = this.moqer.Resolve<RssChannelsService>();
        }

        [Test]
        public void T001_When_Loading_AllChannels_Then_LoadFromRepositoryIsCalled()
        {
            // arrange 

            this.mockRssChannelsRepository.Setup(x => x.LoadAllChannelsWithStatistics())
                .Returns(new List<Dtos.RssChannelSubscriptionWithStatisticsData>
                {
                    new Dtos.RssChannelSubscriptionWithStatisticsData(),
                    new Dtos.RssChannelSubscriptionWithStatisticsData(),
                    new Dtos.RssChannelSubscriptionWithStatisticsData()
                });

            // act
            sut.LoadAllChannels();

            // assert
            this.mockRssChannelsRepository.Verify(x => x.LoadAllChannelsWithStatistics(), Times.Once());
        }

        [Test]
        public void T002_When_Repository_Returned_Values_Then_Automapper_Is_Called()
        {
            // arrange 
            this.mockRssChannelsRepository.Setup(x => x.LoadAllChannelsWithStatistics())
                .Returns(new List<Dtos.RssChannelSubscriptionWithStatisticsData>
                {
                    new Dtos.RssChannelSubscriptionWithStatisticsData(),
                    new Dtos.RssChannelSubscriptionWithStatisticsData(),
                    new Dtos.RssChannelSubscriptionWithStatisticsData()
                });

            // act
            sut.LoadAllChannels();

            // assert
            this.mockMapper.Verify(
                x => x.Map<RssChannelsIndexViewModel>(It.IsAny<List<Dtos.RssChannelSubscriptionWithStatisticsData>>()),
                Times.Once());
        }
    }
}
