using System.Collections.Generic;
using AutoMapper;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests
{
    public class RssChannelsService_CreateNewChannelIfNotExists_Tests
    {
        private AutoMoqer moqer;
        private RssChannelsService sut;
        private Mock<ISessionProvider> mockSessionProvider;
        private Mock<IRssChannelsSubscriptionsRepository> mockRssSubscriptionRepository;
        private Mock<IMapper> mockMapper;
        private Mock<IRssChannelsRepository> mockChannelRepository;

        public RssChannelsService_CreateNewChannelIfNotExists_Tests()
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

            this.mockChannelRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(new List<long>());

            // act
            this.sut.CreateNewChannelIfNotExists(dto);

            // assert
            this.mockChannelRepository
                .Verify(v => v.GetIdByChannelUrl(It.IsAny<List<string>>()),
                    Times.Once());

        }
    }
}