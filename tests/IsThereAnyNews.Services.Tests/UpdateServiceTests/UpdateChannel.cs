﻿namespace IsThereAnyNews.Services.Tests.UpdateServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

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
        private Mock<IMapper> mockMapper;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.mockUpdateRepository = this.moqer.GetMock<IUpdateRepository>();
            this.mockRssChannelRepository = this.moqer.GetMock<IRssChannelsRepository>();
            this.mockSyndicationFeedAdapter = this.moqer.GetMock<ISyndicationFeedAdapter>();
            this.mockRssEntriesRepository = this.moqer.GetMock<IRssEntriesRepository>();
            this.mockMapper = this.moqer.GetMock<IMapper>();
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

        [Test]
        public void T001_GivenRssChannelWithNoNewRss_WhenUpdating_ThenCannotPassItToMapperForProjection()
        {
            // arrange
            var rssChannelForUpdateDto = new RssChannelForUpdateDTO { RssLastUpdatedTime = new DateTime(2000, 1, 2) };
            var syndicationItemAdapter = new SyndicationItemAdapter
            {
                Id = "1",
                Url = "dummy",
                PublishDate = new DateTime(2000, 1, 1)
            };

            var syndicationItemAdapters = new List<SyndicationItemAdapter> { syndicationItemAdapter };

            this.mockMapper.Setup(
                s =>
                    s.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(
                        It.IsAny<IEnumerable<SyndicationItemAdapter>>(),
                        It.IsAny<List<NewRssEntryDTO>>())).Returns(new List<NewRssEntryDTO>());

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

            this.mockMapper.Verify(v =>
                    v.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(
                        It.Is<IEnumerable<SyndicationItemAdapter>>(p => !p.Any())),
                Times.Once);
        }

        [Test]
        public void T002_GivenRssChannelWithNewRss_WhenUpdating_ThenMustBeStoredToDatabase()
        {
            // arrange
            var rssChannelForUpdateDto = new RssChannelForUpdateDTO { RssLastUpdatedTime = new DateTime(2000, 1, 1) };
            var syndicationItemAdapter = new SyndicationItemAdapter
            {
                Id = "1",
                Url = "dummy",
                PublishDate = new DateTime(2000, 1, 2)
            };

            var syndicationItemAdapters = new List<SyndicationItemAdapter> { syndicationItemAdapter };

            this.mockMapper.Setup(
                s =>
                    s.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(
                        It.IsAny<IEnumerable<SyndicationItemAdapter>>(),
                        It.IsAny<List<NewRssEntryDTO>>())).Returns(new List<NewRssEntryDTO>());

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

            this.mockMapper.Verify(v =>
                    v.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(
                        It.Is<IEnumerable<SyndicationItemAdapter>>(p => p.Count() == 1)),
                Times.Once);
        }

        [Test]
        public void T002_GivenRssChannelWithNewAndOldRss_WhenUpdating_ThenOnlyNewEntriesMustBeStoredToDatabase()
        {
            // arrange
            var rssChannelForUpdateDto = new RssChannelForUpdateDTO { RssLastUpdatedTime = new DateTime(2000, 1, 2) };
            var syndicationItemAdapter1 = new SyndicationItemAdapter
            {
                Id = "1",
                Url = "dummy",
                PublishDate = new DateTime(2000, 1, 1)
            };

            var syndicationItemAdapter2 = new SyndicationItemAdapter
            {
                Id = "2",
                Url = "dummy",
                PublishDate = new DateTime(2000, 1, 3)
            };

            var syndicationItemAdapters = new List<SyndicationItemAdapter>
                                              {
                                                  syndicationItemAdapter1,
                                                  syndicationItemAdapter2
                                              };

            this.mockSyndicationFeedAdapter
                .Setup(s => s.Load(It.IsAny<string>()))
                .Returns(syndicationItemAdapters);

            this.mockMapper.Setup(
                s =>
                    s.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(
                        It.IsAny<IEnumerable<SyndicationItemAdapter>>(),
                        It.IsAny<List<NewRssEntryDTO>>())).Returns(new List<NewRssEntryDTO>());

            // act
            this.sut.UpdateChannel(rssChannelForUpdateDto);

            // assert
            this.mockSyndicationFeedAdapter
                .Verify(v => v.Load(It.IsAny<string>()), Times.Once);

            this.mockRssEntriesRepository
                .Verify(v => v.SaveToDatabase(
                    It.IsAny<List<NewRssEntryDTO>>()),
                Times.Once);

            this.mockChannelsUpdateRepository
                .Verify(v => v.SaveEvent(It.IsAny<long>()), Times.Once);

            this.mockMapper.Verify(v =>
                    v.Map<IEnumerable<SyndicationItemAdapter>, List<NewRssEntryDTO>>(
                        It.Is<IEnumerable<SyndicationItemAdapter>>(p => p.Count() == 1 && p.Count(i => i.Id == "2") == 1)),
                Times.Once);
        }
    }
}