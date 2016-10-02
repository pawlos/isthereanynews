namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    using AutoMoq;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services.Implementation;
    using IsThereAnyNews.SharedData;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class MarkRead
    {
        private AutoMoqer moqer;
        private RssSubscriptionService sut;
        private Mock<ISubscriptionHandlerFactory> mockSubscriptionHandlerFactory;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockSubscriptionHandlerFactory = this.moqer.GetMock<ISubscriptionHandlerFactory>();
        }

        [Test]
        public void Marking_As_Read_Must_Load_Handler_From_Factory()
        {
            // arrange
            var stub = new MarkReadDto
            {
                DisplayedItems = string.Empty,
                Id = 0,
                StreamType = StreamType.Person
            };

            var mockHandler = new Mock<ISubscriptionHandler>();

            this.mockSubscriptionHandlerFactory
                .Setup(s => s.GetProvider(It.IsAny<StreamType>()))
                .Returns(mockHandler.Object);

            // act
            this.sut.MarkRead(stub);

            // assert
            this.mockSubscriptionHandlerFactory
                .Verify(v => v.GetProvider(It.IsAny<StreamType>()),
                    Times.Once);
        }

        [Test]
        public void Marking_As_Read_Must_Call_Mark_Read_On_Handler()
        {
            // arrange
            var stub = new MarkReadDto
            {
                DisplayedItems = string.Empty,
                Id = 0,
                StreamType = StreamType.Person
            };

            var mockHandler = new Mock<ISubscriptionHandler>();

            this.mockSubscriptionHandlerFactory
                .Setup(s => s.GetProvider(It.IsAny<StreamType>()))
                .Returns(mockHandler.Object);

            // act
            this.sut.MarkRead(stub);

            // assert
            mockHandler
                .Verify(v => v.MarkRead(It.IsAny<string>()),
                Times.Once);
        }
    }
}