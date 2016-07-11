namespace IsThereAnyNews.Services.Tests.UpdateServiceTests
{
    using System.Collections.Generic;

    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.Services.Implementation;

    using Moq;

    using Xunit;

    public class UpdateGlobalRss
    {
        private readonly AutoMoqer moqer;
        private readonly UpdateService sut;

        private readonly Mock<IUpdateRepository> mockUpdateRepository;
        private readonly Mock<IRssChannelsRepository> mockRssChannelsRepository;

        public UpdateGlobalRss()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<UpdateService>();

            this.mockUpdateRepository = this.moqer.GetMock<IUpdateRepository>();
            this.mockRssChannelsRepository = this.moqer.GetMock<IRssChannelsRepository>();
        }

        [Fact]
        public void T001_Given_No_Rss_Was_Loaded_Must_Call_Update_Last_Read_Of_Channels_With_Empty_List()
        {
            // arrange
            this.mockUpdateRepository
                .Setup(s => s.LoadAllGlobalRssChannels())
                .Returns(new List<RssChannel>());

            // act
            this.sut.UpdateGlobalRss();

            // assert
            this.mockRssChannelsRepository.Verify(
                v => v.UpdateRssLastUpdateTimeToDatabase(It.Is<List<RssChannel>>(p => p.Count == 0)),
                Times.Once());
        }

        [Fact]
        public void T002_Given_Rss_Channel_Is_Loaded()
        {
            // arrange
            this.mockUpdateRepository
                .Setup(s => s.LoadAllGlobalRssChannels())
                .Returns(new List<RssChannel>() { new RssChannel { Url = "www.url.com" } });

            // act
            this.sut.UpdateGlobalRss();

            

        }
    }
}