using AutoMoq;
using IsThereAnyNews.Services.Implementation;
using IsThereAnyNews.SharedData;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    public class LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription
    {
        private readonly AutoMoqer moqer;
        private readonly RssSubscriptionService sut;
        private readonly Mock<ISubscriptionHandlerFactory> mockHandlerFactory;
        private readonly Mock<ISubscriptionHandler> mockSubscriptionHandler;

        public LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockHandlerFactory = this.moqer.GetMock<ISubscriptionHandlerFactory>();
            this.mockSubscriptionHandler = this.moqer.GetMock<ISubscriptionHandler>();
        }

        [Theory]
        [InlineData(StreamType.Person)]
        [InlineData(StreamType.Rss)]
        public void When_Loading_Unread_Entries_Must_Call_To_Factory_For_Proper_Handler(StreamType streamType)
        {
            // assert
            this.mockHandlerFactory
                .Setup(s => s.GetProvider(It.IsAny<StreamType>()))
                .Returns(this.mockSubscriptionHandler.Object);

            // act
            this.sut.LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType, 1, ShowReadEntries.Show);

            // assert
            this.mockHandlerFactory
                .Verify(v => v.GetProvider(It.IsAny<StreamType>()),
                Times.Once);
        }

        [Theory]
        [InlineData(StreamType.Person)]
        [InlineData(StreamType.Rss)]
        public void When_Provider_Is_Provided_Must_Pass_Parameters_To_Provider(StreamType streamType)
        {
            // assert
            this.mockHandlerFactory
                .Setup(s => s.GetProvider(It.IsAny<StreamType>()))
                .Returns(this.mockSubscriptionHandler.Object);

            // act
            this.sut.LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType, 1, ShowReadEntries.Show);

            // assert
            this.mockSubscriptionHandler
                .Verify(v => v.GetSubscriptionViewModel(It.IsAny<long>(), It.IsAny<ShowReadEntries>()),
                Times.Once);
        }
    }
}
