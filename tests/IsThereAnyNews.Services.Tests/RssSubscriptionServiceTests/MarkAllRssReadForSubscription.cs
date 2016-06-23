using System.Collections.Generic;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.RssSubscriptionServiceTests
{
    public class MarkAllRssReadForSubscription
    {
        private readonly AutoMoqer moqer;
        private readonly RssSubscriptionService sut;
        private readonly Mock<IRssEntriesToReadRepository> mockRssToReadRepository;

        public MarkAllRssReadForSubscription()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<RssSubscriptionService>();
            this.mockRssToReadRepository = this.moqer.GetMock<IRssEntriesToReadRepository>();
        }

        [Fact]
        public void When_Passed_String_Of_Separated_Ids_Then_Must_Split_And_Parse()
        {
            // arrange
            var dto = new MarkReadForSubscriptionDto
            {
                RssEntries = "1;321;666666",
                SubscriptionId = 0
            };

            // act
            this.sut.MarkAllRssReadForSubscription(dto);

            // assert
            this.mockRssToReadRepository
                .Verify(v => v.MarkAllReadForUserAndSubscription(
                    It.Is<long>(p => p == 0),
                    It.Is<List<long>>(p => p.Count == 3 &&
                                           p[0] == 1 &&
                                           p[1] == 321 &&
                                           p[2] == 666666)),
                Times.Once);
        }

        [Fact]
        public void When_Marking_As_Read_Must_Call_Repository_To_Update_Entries()
        {
            // arrange
            var dto = new MarkReadForSubscriptionDto
            {
                RssEntries = "1;321;666666",
                SubscriptionId = 0
            };

            // act
            this.sut.MarkAllRssReadForSubscription(dto);

            // assert
            this.mockRssToReadRepository
                .Verify(v => v.MarkAllReadForUserAndSubscription(
                    It.IsAny<long>(),
                    It.IsAny<List<long>>()),
                Times.Once);
        }
    }
}