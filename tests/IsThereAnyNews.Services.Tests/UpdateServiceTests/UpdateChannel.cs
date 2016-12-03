namespace IsThereAnyNews.Services.Tests.UpdateServiceTests
{
    using System.Collections.Generic;

    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.Services.Implementation;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class UpdateChannel
    {
        private AutoMoqer moqer;
        private UpdateService sut;
        private Mock<IUpdateRepository> mockUpdateRepository;
        private Mock<IRssChannelsRepository> mockRssChannelRepository;
        private Mock<ISyndicationFeedAdapter> mockSyndicationFeedAdapter;
        private Mock<IRssEntriesRepository> mockRssEntriesRepository;
        private Mock<IRssChannelUpdateRepository> mockChannelsUpdateRepository;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.mockUpdateRepository = this.moqer.GetMock<IUpdateRepository>();
            this.mockRssChannelRepository = this.moqer.GetMock<IRssChannelsRepository>();
            this.mockSyndicationFeedAdapter = this.moqer.GetMock<ISyndicationFeedAdapter>();
            this.mockRssEntriesRepository = this.moqer.GetMock<IRssEntriesRepository>();
            this.mockChannelsUpdateRepository = this.moqer.GetMock<IRssChannelUpdateRepository>();

            this.sut = this.moqer.Resolve<UpdateService>();
        }

        [Test]
        public void T000_GivenRssChannel_WhenUpdating_ThenMustLoadRssEntriesSaveEntriesAndGenerateEvent()
        {
            // arrange
            var rssChannelForUpdateDto = new RssChannelForUpdateDTO();
            var syndicationItemAdapters = new List<SyndicationItemAdapter>();

            this.mockSyndicationFeedAdapter
                .Setup(s => s.Load(It.IsAny<string>()))
                .Returns(syndicationItemAdapters);

            // act
            this.sut.UpdateChannel(rssChannelForUpdateDto);

            // assert
            this.mockSyndicationFeedAdapter
                .Verify(v => v.Load(It.IsAny<string>()), Times.Once);

            this.mockRssEntriesRepository
                .Verify(v => v.SaveToDatabase(It.IsAny<List<NewRssEntryDTO>>()), Times.Once);

            this.mockChannelsUpdateRepository
                .Verify(v => v.SaveEvent(It.IsAny<long>()), Times.Once);
        }
    }
}