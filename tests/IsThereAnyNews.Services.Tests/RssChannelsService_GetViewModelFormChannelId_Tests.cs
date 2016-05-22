using System.Collections.Generic;
using AutoMapper;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.Services.Implementation;
using IsThereAnyNews.ViewModels;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests
{
    public class RssChannelsService_GetViewModelFormChannelId_Tests
    {
        private AutoMoqer moqer;
        private RssChannelsService sut;
        private Mock<IRssChannelsRepository> mockChannelsRepository;
        private Mock<IMapper> mockMapper;

        public RssChannelsService_GetViewModelFormChannelId_Tests()
        {
            this.moqer = new AutoMoq.AutoMoqer();

            this.mockChannelsRepository = this.moqer.GetMock<IRssChannelsRepository>();
            this.mockMapper = this.moqer.GetMock<IMapper>();

            this.sut = this.moqer.Resolve<RssChannelsService>();
        }

        [Fact]
        public void T001_When_Creating_RssChannelIndexViewModel_From_Channel_Id_Then_Channel_Must_Be_Read_From_Repository()
        {
            // arrange
            var channelId = 33;

            this.mockChannelsRepository
                .Setup(s => s.LoadRssChannel(It.IsAny<long>()))
                .Returns(new RssChannel());

            // act
            this.sut.GetViewModelFormChannelId(channelId);

            // assert 
            this.mockChannelsRepository.Verify(v => v.LoadRssChannel(It.Is<long>(p => p == 33)), Times.Once());
        }

        [Fact]
        public void T002_When_Repository_Returns_Model_Then_Automapper_Must_Be_Called()
        {
            // arrange
            var channelId = 33;

            this.mockChannelsRepository
                .Setup(s => s.LoadRssChannel(It.IsAny<long>()))
                .Returns(new RssChannel());

            // act
            this.sut.GetViewModelFormChannelId(channelId);

            // assert 
            this.mockMapper
                .Verify(v => v.Map<RssChannelIndexViewModel>(It.IsAny<RssChannel>()), Times.Once());
        }
    }
}