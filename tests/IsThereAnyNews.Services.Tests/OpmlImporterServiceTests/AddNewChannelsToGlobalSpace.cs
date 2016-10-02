namespace IsThereAnyNews.Services.Tests.OpmlImporterServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.Services.Implementation;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class AddNewChannelsToGlobalSpace
    {
        private AutoMoqer moqer;
        private OpmlImporterService sut;
        private Mock<IRssChannelsRepository> mockRssChannelsRepository;
        private Mock<IRssChannelsSubscriptionsRepository> mockRssSubscriptionsRepository;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<OpmlImporterService>();
            this.mockRssChannelsRepository = this.moqer.GetMock<IRssChannelsRepository>();
            this.mockRssSubscriptionsRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
        }

        [Test]
        public void T001_When_Importing_From_List_Of_Channels_Then_Must_Load_RssChannels_From_Repository()
        {
            // arrange

            // act
            this.sut.AddNewChannelsToGlobalSpace(new List<RssChannel>());

            // assert
            this.mockRssSubscriptionsRepository
                .Verify(v => v.LoadUrlsForAllChannels(),
                    Times.Once());
        }

        [Test]
        public void T002_When_There_Are_New_Channels_Then_Must_Save_Them_To_Repository()
        {
            // arrange
            var newList = new List<RssChannel> { new RssChannel { Url = "not-existing", Id = 1 }, new RssChannel { Url = "existing", Id = 2 } };
            var oldList = new List<RssChannel> { new RssChannel { Url = "existing", Id = 2 } };

            this.mockRssSubscriptionsRepository
                .Setup(s => s.LoadUrlsForAllChannels())
                .Returns(oldList.Select(s => s.Url).ToList());

            // act
            this.sut.AddNewChannelsToGlobalSpace(newList);

            // assert
            this.mockRssChannelsRepository
                .Verify(v => v.SaveToDatabase(It.Is<List<RssChannel>>(p => p.Count == 1 && p.Single().Id == 1)),
                    Times.Once());
        }

    }
}