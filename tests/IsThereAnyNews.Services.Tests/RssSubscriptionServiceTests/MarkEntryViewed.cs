namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    using AutoMoq;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services.Implementation;
    using IsThereAnyNews.SharedData;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class MarkEntryViewed
    {
        private AutoMoqer moqer;
        private RssSubscriptionService sut;
        private Mock<ISubscriptionHandlerFactory> mockSubscriptionHandlerFactory;


        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockSubscriptionHandlerFactory = this.moqer.GetMock<ISubscriptionHandlerFactory>();
        }

        [Test]
        public void T001_Marking_Rss_As_Read_Must_Mark_It_Via_Repository()
        {
            // assert
            var mockSubscriptionHandler = this.moqer.GetMock<ISubscriptionHandler>();

            this.mockSubscriptionHandlerFactory
                .Setup(s => s.GetProvider(It.IsAny<StreamType>()))
                .Returns(mockSubscriptionHandler.Object);

            var markReadDto = new MarkReadDto { StreamType = StreamType.Rss, Id = 0, DisplayedItems = "0" };

            // act
            this.sut.MarkRead(markReadDto);

            // assert
            mockSubscriptionHandler.Verify(v => v.MarkRead(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void T002_Marking_As_Read_Must_Fetch_Handler_For_Proper_Stream_Type()
        {
            // assert
            var mockSubscriptionHandler = this.moqer.GetMock<ISubscriptionHandler>();

            this.mockSubscriptionHandlerFactory
                .Setup(s => s.GetProvider(It.IsAny<StreamType>()))
                .Returns(mockSubscriptionHandler.Object);

            var markReadDto = new MarkReadDto { StreamType = StreamType.Rss, Id = 0, DisplayedItems = "0" };

            // act
            this.sut.MarkRead(markReadDto);

            // assert
            this.mockSubscriptionHandlerFactory
                .Verify(v => v.GetProvider(It.Is<StreamType>(p => p == StreamType.Rss)),
                Times.Once);
        }

        [Test]
        public void T003_Marking_As_Read_Must_Create_An_Event_Viewed_With_Id_Of_That_Rss()
        {
            // assert
            var mockSubscriptionHandler = this.moqer.GetMock<ISubscriptionHandler>();

            this.mockSubscriptionHandlerFactory
                .Setup(s => s.GetProvider(It.IsAny<StreamType>()))
                .Returns(mockSubscriptionHandler.Object);

            var markReadDto = new MarkReadDto { StreamType = StreamType.Rss, Id = 3345, DisplayedItems = "0" };

            // act
            this.sut.MarkRead(markReadDto);

            // assert
            mockSubscriptionHandler.Verify(v => v.AddEventViewed(It.Is<long>(p => p == 3345)));
        }

    }
}