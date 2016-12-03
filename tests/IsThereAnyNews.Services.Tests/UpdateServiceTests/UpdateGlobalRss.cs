namespace IsThereAnyNews.Services.Tests.UpdateServiceTests
{
    using System.Collections.Generic;

    using AutoMapper;

    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.Services.Implementation;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class UpdateGlobalRss
    {
        private AutoMoqer moqer;
        private Mock<IMapper> mockMapper;
        private UpdateService sut;
        private Mock<IUpdateRepository> mockUpdateRepository;
        private Mock<IRssChannelsRepository> mockRssChannelRepository;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.mockUpdateRepository = this.moqer.GetMock<IUpdateRepository>();
            this.mockRssChannelRepository = this.moqer.GetMock<IRssChannelsRepository>();

            this.sut = this.moqer.Resolve<UpdateService>();
        }

        [Test]
        public void T001_GivenGlobalChannels_WhenUpdated_ThenMustUpdateChannelstoDatabase()
        {
            // arrange
            var rssChannelForUpdateDtos = new List<RssChannelForUpdateDTO>();
            this.mockUpdateRepository
                .Setup(s => s.LoadAllGlobalRssChannels())
                .Returns(rssChannelForUpdateDtos);

            // act
            this.sut.UpdateGlobalRss();

            // assert
            this.mockUpdateRepository
                .Verify(v => v.LoadAllGlobalRssChannels(), Times.Once);

            this.mockRssChannelRepository
                .Verify(v => v.UpdateRssLastUpdateTimeToDatabase(It.IsAny<List<long>>()), Times.Once);
        }
    }
}
