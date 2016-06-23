using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    public class MarkEntryViewed
    {
        private readonly AutoMoqer moqer;
        private readonly RssSubscriptionService sut;
        private readonly Mock<IRssEntriesToReadRepository> mockRssToReadRepository;
        private Mock<IRssEventRepository> mockRssEventRepository;
        private Mock<ISessionProvider> mockSessionProvider;

        public MarkEntryViewed()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockRssToReadRepository = this.moqer.GetMock<IRssEntriesToReadRepository>();
            this.mockRssEventRepository = this.moqer.GetMock<IRssEventRepository>();
            this.mockSessionProvider = this.moqer.GetMock<ISessionProvider>();
        }

        [Fact]
        public void Marking_Rss_As_Read_Must_Mark_It_Via_Repository()
        {
            // act
            this.sut.MarkEntryViewed(0);

            // assert
            this.mockRssToReadRepository
                .Verify(v => v.MarkEntryViewedByUser(It.IsAny<long>(), It.IsAny<long>()),
                    Times.Once);
        }

        [Fact]
        public void Marking_Rss_As_Read_Must_Load_Current_User_Id_From_Session()
        {
            // act
            this.sut.MarkEntryViewed(0);

            // assert
            this.mockSessionProvider
                .Verify(v => v.GetCurrentUserId(),
                    Times.Once);
        }

        [Fact]
        public void Marking_Rss_As_Read_Must_Generate_Event_Rss_Viewed()
        {
            // act
            this.sut.MarkEntryViewed(0);

            // assert
            this.mockRssEventRepository
                .Verify(v => v.AddEventRssViewed(It.IsAny<long>(), It.IsAny<long>()),
                Times.Once);
        }
    }
}