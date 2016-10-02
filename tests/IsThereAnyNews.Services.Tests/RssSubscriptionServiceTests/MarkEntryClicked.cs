namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services.Implementation;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class MarkEntryClicked
    {
        private AutoMoqer moqer;
        private RssSubscriptionService sut;
        private Mock<ISessionProvider> mockSessionProvider;
        private Mock<IRssEventRepository> mockRssEventRepository;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
            this.mockRssEventRepository = this.moqer.GetMock<IRssEventRepository>();
        }

        [Test]
        public void Marking_As_Clicked_Must_Load_Current_User_From_Session()
        {
            // arrange
            var stub = new MarkClickedDto
            {
                Id = 0
            };

            // act
            this.sut.MarkClicked(stub);

            // assert
            this.mockSessionProvider
                .Verify(v => v.GetCurrentUserId(),
                    Times.Once);
        }

        [Test]
        public void Marking_As_Clicked_Must_Generate_Event()
        {
            // arrange
            var stub = new MarkClickedDto
            {
                Id = 0
            };

            // act
            this.sut.MarkClicked(stub);

            // assert
            this.mockRssEventRepository
                .Verify(v => v.MarkClicked(It.IsAny<long>(), It.IsAny<long>()),
                    Times.Once);
        }
    }
}