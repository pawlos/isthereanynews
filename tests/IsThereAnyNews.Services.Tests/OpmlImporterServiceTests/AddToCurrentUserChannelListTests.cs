using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Xml;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.OpmlImporterServiceTests
{
    public class AddToCurrentUserChannelList
    {
        private readonly AutoMoqer moqer;
        private readonly OpmlImporterService sut;
        private readonly Mock<IRssChannelsRepository> mockRssChannelsRepository;
        private readonly Mock<IRssChannelsSubscriptionsRepository> mockRssSubscriptionsRepository;
        private readonly Mock<IOpmlReader> mockOpmlReader;

        public AddToCurrentUserChannelList()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<OpmlImporterService>();
            this.mockRssChannelsRepository = this.moqer.GetMock<IRssChannelsRepository>();
            this.mockRssSubscriptionsRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.mockOpmlReader = this.moqer.GetMock<IOpmlReader>();
        }

        [Fact]
        public void T001_When_Importing_RssChannels_Must_Load_Find_Existing_Channels()
        {
            // arrange
            this.mockRssChannelsRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(new List<long>());

            // act
            this.sut.AddToCurrentUserChannelList(new List<RssChannel>());

            // assert
            this.mockRssChannelsRepository
                .Verify(v => v.GetIdByChannelUrl(It.IsAny<List<string>>()),
                    Times.Once());
        }

        [Fact]
        public void T002_When_Importing_RssChannels_Must_Convert_All_Urls_To_Lower_Case()
        {
            // arrange
            this.mockRssChannelsRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(new List<long>());

            var channels =
                new List<RssChannel>
                {
                    new RssChannel {Title = "blah1", Url = "URL1"},
                    new RssChannel {Title = "blah2", Url = "Url2"},
                    new RssChannel {Title = "blah3", Url = "url3"},
                };

            // act
            this.sut.AddToCurrentUserChannelList(channels);

            // assert
            this.mockRssChannelsRepository
                .Verify(
                    v => v.GetIdByChannelUrl(
                            It.Is<List<string>>(p => p.Contains("url1") && p.Contains("url2") && p.Contains("url3"))),
                    Times.Once());
        }

        [Fact]
        public void T003_When_Repository_Returns_Ids_Then_Must_Save_Only_These_That_Are_Not_Subscribed_Yet()
        {
            // arrange
            this.mockRssChannelsRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(new List<long> { 1, 2 });

            var channels =
                new List<RssChannel>
                {
                    new RssChannel {Title = "blah1", Url = "URL1", Id = 1},
                    new RssChannel {Title = "blah2", Url = "Url2", Id = 2},
                    new RssChannel {Title = "blah3", Url = "url3", Id = 3},
                };

            this.mockRssSubscriptionsRepository
                .Setup(s => s.GetChannelIdSubscriptionsForUser(It.IsAny<long>()))
                .Returns(new List<long> { 2, 3 });

            // act
            this.sut.AddToCurrentUserChannelList(channels);

            // assert
            this.mockRssSubscriptionsRepository
                .Verify(
                    v => v.SaveToDatabase(
                        It.Is<List<RssChannelSubscription>>(p => p.Count == 1 && p.Single().RssChannelId == 1)),
                    Times.Once());
        }

        [Fact]
        public void T004_When_Importing_RssChannels_From_Opml_File_Then_Must_Load_Nodes_Using_Opml_Importer()
        {
            // arrange
            this.mockOpmlReader
                .Setup(s => s.GetOutlines(It.IsAny<Stream>()))
                .Returns(new List<XmlNode>());

            var x = new Mock<HttpPostedFileBase>();


            var opmlImporterIndexDto = new OpmlImporterIndexDto
            {
                ImportFile = x.Object
            };

            // act
            this.sut.ParseToRssChannelList(opmlImporterIndexDto);

            // arrange
            this.mockOpmlReader
                .Verify(v => v.GetOutlines(It.IsAny<Stream>()),
                Times.Once());
        }

        [Fact]
        public void T005_When_Xml_Node_Doesnt_Have_A_Title_Then_Its_Invalid()
        {
            // arrange
            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><body><outline><outline title=\"Programming\"/><outline  xmlUrl=\"http://jczraiby.wordpress.com/feed/\" /></outline></body>";
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var outlines = xmlDocument.GetElementsByTagName("outline");
            var nodes = outlines.Cast<XmlNode>();

            // act
            var rssList = this.sut.FilterOutInvalidOutlines(nodes);

            // arrange
            Assert.Equal(0, rssList.Count);
        }

        [Fact]
        public void T006_When_Xml_Node_Doesnt_Have_A_Url_Then_Its_Invalid()
        {
            // arrange
            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><body><outline><outline text=\"Programming\" title=\"Programming\"/><outline  title=\"Johnny Zraiby\" /></outline></body>";
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var outlines = xmlDocument.GetElementsByTagName("outline");
            var nodes = outlines.Cast<XmlNode>();

            // act
            var rssList = this.sut.FilterOutInvalidOutlines(nodes);

            // arrange
            Assert.Equal(0, rssList.Count);
        }

        [Fact]
        public void T007_When_Xml_Node_Has_An_Url_And_Title_Then_Its_Valid()
        {
            // arrange
            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><body><outline><outline text=\"Programming\" title=\"Programming\"/><outline  title=\"Johnny Zraiby\" xmlUrl=\"http://jczraiby.wordpress.com/feed/\" /></outline></body>";
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var outlines = xmlDocument.GetElementsByTagName("outline");
            var nodes = outlines.Cast<XmlNode>();

            // act
            var rssList = this.sut.FilterOutInvalidOutlines(nodes);

            // arrange
            Assert.Equal(1, rssList.Count);
        }

        [Fact]
        public void T007_When_When_Node_List_Contains_Valid_Node_Then_All_Of_Then_Must_Be_Returned_From_Parsing_As_Channels()
        {
            // arrange

            var xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><body><outline><outline text=\"Programming\" title=\"Programming\"/><outline  title=\"Johnny Zraiby\" xmlUrl=\"http://jczraiby.wordpress.com/feed/\" /></outline></body>";
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            var outlines = xmlDocument.GetElementsByTagName("outline");
            var nodes = outlines.Cast<XmlNode>();

            this.mockOpmlReader
                .Setup(s => s.GetOutlines(It.IsAny<Stream>()))
                .Returns(nodes);

            var x = new Mock<HttpPostedFileBase>();

            var opmlImporterIndexDto = new OpmlImporterIndexDto
            {
                ImportFile = x.Object
            };

            // act
            var rssList = this.sut.ParseToRssChannelList(opmlImporterIndexDto);

            // arrange
            Assert.Equal(1, rssList.Count);
        }
    }
}