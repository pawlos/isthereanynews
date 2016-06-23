using System.Collections.Generic;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    public class SubscribeCurrentUserToChannel
    {
        private readonly AutoMoqer moqer;
        private readonly RssSubscriptionService sut;
        private readonly Mock<ISessionProvider> mockSessionProvider;
        private readonly Mock<IRssChannelsSubscriptionsRepository> mockRssChannelSubscriptionRepository;
        private readonly Mock<IRssChannelsRepository> mockRssChannelRepository;

        public SubscribeCurrentUserToChannel()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
            this.mockRssChannelSubscriptionRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
            this.mockRssChannelRepository = this.moqer.GetMock<IRssChannelsRepository>();
        }

        [Fact]
        public void Subscribing_To_Channel_Must_Load_Current_User_From_Session()
        {
            // arrange
            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(0);

            this.mockRssChannelSubscriptionRepository
                .Setup(s => s.IsUserSubscribedToChannelUrl(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(true);

            this.mockRssChannelRepository
                .Setup(s => s.Load(It.IsAny<long>()))
                .Returns(new RssChannel());

            // act
            this.sut.SubscribeCurrentUserToChannel(0);

            // assert
            this.mockSessionProvider
                .Verify(v => v.GetCurrentUserId(),
                    Times.Once);
        }

        [Fact]
        public void Subscribing_To_Channel_Must_Load_Channel_From_Repository()
        {
            // arrange
            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(0);

            this.mockRssChannelSubscriptionRepository
                .Setup(s => s.IsUserSubscribedToChannelUrl(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(true);

            this.mockRssChannelRepository
                .Setup(s => s.Load(It.IsAny<long>()))
                .Returns(new RssChannel());

            // act
            this.sut.SubscribeCurrentUserToChannel(0);

            // assert
            this.mockRssChannelRepository
                .Verify(v => v.Load(It.IsAny<long>()),
                    Times.Once);
        }

        [Fact]
        public void When_Subscribing_To_Subscribed_Channel_Must_Not_Update_Database_With_New_Subscription()
        {
            // arrange
            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(0);

            this.mockRssChannelSubscriptionRepository
                .Setup(s => s.IsUserSubscribedToChannelUrl(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(true);

            this.mockRssChannelRepository
                .Setup(s => s.Load(It.IsAny<long>()))
                .Returns(new RssChannel());

            // act
            this.sut.SubscribeCurrentUserToChannel(0);

            // assert
            this.mockRssChannelSubscriptionRepository
                .Verify(v => v.SaveToDatabase(It.IsAny<List<RssChannelSubscription>>()),
                Times.Never());
        }

        [Fact]
        public void When_Subscribing_To_Not_Subscribed_Channel_Must_Update_Database_With_New_Subscription()
        {
            // arrange
            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(0);

            this.mockRssChannelSubscriptionRepository
                .Setup(s => s.IsUserSubscribedToChannelUrl(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(false);

            this.mockRssChannelRepository
                .Setup(s => s.Load(It.IsAny<long>()))
                .Returns(new RssChannel());

            this.mockRssChannelRepository
               .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
               .Returns(new List<long>() { 0 });

            // act
            this.sut.SubscribeCurrentUserToChannel(0);

            // assert
            this.mockRssChannelSubscriptionRepository
                .Verify(v => v.SaveToDatabase(It.IsAny<List<RssChannelSubscription>>()),
                Times.Once);
        }


        [Fact]
        public void When_Subscribing_To_Not_Subscribed_Channel_Must_Load_Channel_By_Url()
        {
            // arrange
            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(0);

            this.mockRssChannelSubscriptionRepository
                .Setup(s => s.IsUserSubscribedToChannelUrl(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(false);

            this.mockRssChannelRepository
                .Setup(s => s.Load(It.IsAny<long>()))
                .Returns(new RssChannel());

            this.mockRssChannelRepository
                .Setup(s => s.GetIdByChannelUrl(It.IsAny<List<string>>()))
                .Returns(new List<long>() { 0 });

            // act
            this.sut.SubscribeCurrentUserToChannel(0);

            // assert
            this.mockRssChannelRepository
                .Verify(v => v.GetIdByChannelUrl(It.IsAny<List<string>>()),
                Times.Once);
        }
    }
}