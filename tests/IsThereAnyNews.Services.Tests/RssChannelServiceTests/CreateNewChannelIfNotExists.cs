using System.Collections.Generic;
using AutoMapper;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssChannelServiceTests
{
    public class CreateNewChannelIfNotExists
    {
        private AutoMoqer moqer;
        private RssChannelsService sut;
        private Mock<ISessionProvider> mockSessionProvider;
        private Mock<IRssChannelsSubscriptionsRepository> mockRssSubscriptionRepository;
        private Mock<IMapper> mockMapper;
        private Mock<IRssChannelsRepository> mockChannelRepository;

        public CreateNewChannelIfNotExists()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssChannelsService>();
            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
            this.mockRssSubscriptionRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.mockMapper = this.moqer.GetMock<IMapper>();
            this.mockChannelRepository = this.moqer.GetMock<IRssChannelsRepository>();
        }

        [Fact]
        public void T001_When_Checking_If_Channel_Exists_Then_Must_Load_It_First_From_Repository()
        {
            // arrange
            var dto = new AddChannelDto();

            var value = new List<long>() { 1, 2, 3 };

            this.mockChannelRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(value);

            // act
            this.sut.CreateNewChannelIfNotExists(dto);

            // assert
            this.mockChannelRepository
                .Verify(v => v.GetIdByChannelUrl(It.IsAny<List<string>>()),
                    Times.Once());
        }

        [Fact]
        public void T002_When_Channel_Does_Not_Exists_Then_New_Channel_Must_Be_Created_Using_Automapper()
        {
            // arrange
            var dto = new AddChannelDto();

            var value = new List<long>();

            this.mockChannelRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(value);

            this.mockChannelRepository
                .Setup(s => s.SaveToDatabase(It.IsAny<List<RssChannel>>()))
                .Callback(() => value.Add(0));

            this.mockMapper
                .Setup(s => s.Map<RssChannel>(It.IsAny<AddChannelDto>()))
                .Returns(new RssChannel(string.Empty, string.Empty));

            // act
            this.sut.CreateNewChannelIfNotExists(dto);

            // assert
            this.mockMapper
                .Verify(v => v.Map<RssChannel>(It.IsAny<AddChannelDto>()),
                Times.Once());
        }

        [Fact]
        public void T003_When_Channel_Does_Not_Exists_Then_New_Channel_Must_Be_Saved_To_Repository()
        {
            // arrange
            var dto = new AddChannelDto();
            var value = new List<long>();

            this.mockChannelRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(value);

            this.mockMapper
                .Setup(v => v.Map<RssChannel>(It.IsAny<AddChannelDto>()))
                .Returns(new RssChannel());

            this.mockChannelRepository
                .Setup(s => s.SaveToDatabase(It.IsAny<List<RssChannel>>()))
                .Callback(() => value.Add(0));

            this.mockMapper
                .Setup(s => s.Map<RssChannel>(It.IsAny<AddChannelDto>()))
                .Returns(new RssChannel(string.Empty, string.Empty));

            // act
            this.sut.CreateNewChannelIfNotExists(dto);

            // assert
            this.mockChannelRepository
                .Verify(v => v.SaveToDatabase(It.Is<List<RssChannel>>(p => p.Count == 1)),
                Times.Once());
        }

        [Fact]
        public void T004_When_Channel_Does_Exists_Then_Channel_Must_Not_Be_Saved_To_Repository_And_Mapper_Must_Not_Be_Called()
        {
            // arrange
            var dto = new AddChannelDto();

            this.mockChannelRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(new List<long> { 1 });

            this.mockMapper
                .Setup(v => v.Map<RssChannel>(It.IsAny<AddChannelDto>()))
                .Returns(new RssChannel());

            // act
            this.sut.CreateNewChannelIfNotExists(dto);

            // assert
            this.mockChannelRepository
                .Verify(v => v.SaveToDatabase(It.IsAny<List<RssChannel>>()),
                Times.Never());
            this.mockMapper
               .Verify(v => v.Map<RssChannel>(It.IsAny<AddChannelDto>()),
               Times.Never());
        }
    }
}