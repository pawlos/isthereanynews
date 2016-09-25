using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    public class MarkEntryClicked
    {
        private readonly AutoMoqer moqer;
        private readonly RssSubscriptionService sut;
        private readonly Mock<ISessionProvider> mockSessionProvider;
        private readonly Mock<IRssEventRepository> mockRssEventRepository;

        public MarkEntryClicked()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
            this.mockRssEventRepository = this.moqer.GetMock<IRssEventRepository>();
        }

        [Fact]
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

        [Fact]
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