using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMoq;
using IsThereAnyNews.DataAccess;
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

        public AddToCurrentUserChannelList()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<OpmlImporterService>();
            this.mockRssChannelsRepository = this.moqer.GetMock<IRssChannelsRepository>();
            this.mockRssSubscriptionsRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
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
    }
}