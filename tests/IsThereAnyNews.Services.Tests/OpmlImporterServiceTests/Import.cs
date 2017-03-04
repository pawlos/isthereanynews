namespace IsThereAnyNews.Services.Tests.OpmlImporterServiceTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml;

    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services.Implementation;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class Import
    {
        private IOpmlImporterService sut;
        private AutoMoqer moqer;
        private Mock<IOpmlReader> mockOpmlReader;
        private Mock<IRssChannelsSubscriptionsRepository> mockSubscriptionRepository;
        private Mock<IRssChannelsRepository> mockRssChannelRepository;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<OpmlImporterService>();

            this.mockOpmlReader = this.moqer.GetMock<IOpmlReader>();
            this.mockSubscriptionRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.mockRssChannelRepository = this.moqer.GetMock<IRssChannelsRepository>();
        }

        [Test]
        public void T001_When_Nothing_New_Was_Added_By_User_No_New_Subscriptions_Must_Be_Defined()
        {
            // arrange
            OpmlImporterIndexDto stub = new OpmlImporterIndexDto();
            stub.ImportFile = new Mock<HttpPostedFileBase>().Object;

            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><body><outline><outline text=\"Programming\" title=\"Programming\"/><outline  title=\"Johnny Zraiby\" xmlUrl=\"http://jczraiby.wordpress.com/feed/\" /></outline></body>";
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var outlines = xmlDocument.GetElementsByTagName("outline");
            var nodes = outlines.Cast<XmlNode>();

            this.mockOpmlReader
                .Setup(s => s.GetOutlines(It.IsAny<Stream>()))
                .Returns(nodes);

            this.mockSubscriptionRepository
                .Setup(s => s.LoadUrlsForAllChannels())
                .Returns(new List<string>());

            this.mockRssChannelRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(new List<long>());

            this.mockSubscriptionRepository
                .Setup(s => s.GetChannelIdSubscriptionsForUser(It.IsAny<long>()))
                .Returns(new List<long>());

            // act
            this.sut.Import(stub);

            // assert
            this.mockSubscriptionRepository
                .Verify(v => v.CreateNewSubscriptionForUserAndChannel(It.IsAny<long>(), It.IsAny<long>()),
                Times.Never);
        }

        [Test]
        public void T002_When_New_Channel_Was_Added_By_User_One_New_Subscriptions_Must_Be_Defined()
        {
            // arrange
            OpmlImporterIndexDto stub = new OpmlImporterIndexDto();
            stub.ImportFile = new Mock<HttpPostedFileBase>().Object;

            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                      + "<body>"
                      + "<outline>"
                      + "<outline text=\"Programming\" title=\"Programming\"/>"
                      + "<outline title=\"Johnny Zraiby\" xmlUrl=\"http://jczraiby.wordpress.com/feed/\" />"
                      + "</outline>"
                      + "</body>";
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var outlines = xmlDocument.GetElementsByTagName("outline");
            var nodes = outlines.Cast<XmlNode>();

            this.mockOpmlReader
                .Setup(s => s.GetOutlines(It.IsAny<Stream>()))
                .Returns(nodes);

            this.mockSubscriptionRepository
                .Setup(s => s.LoadUrlsForAllChannels())
                .Returns(new List<string>());

            this.mockRssChannelRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(new List<long> { 1 });

            this.mockSubscriptionRepository
                .Setup(s => s.GetChannelIdSubscriptionsForUser(It.IsAny<long>()))
                .Returns(new List<long>());

            // act
            this.sut.Import(stub);

            // assert
            this.mockSubscriptionRepository
                .Verify(v => v.CreateNewSubscriptionForUserAndChannel(It.IsAny<long>(), It.IsAny<long>()),
                Times.Once);
        }
    }
}