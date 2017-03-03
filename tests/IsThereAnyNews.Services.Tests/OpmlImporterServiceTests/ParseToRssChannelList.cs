namespace IsThereAnyNews.Services.Tests.OpmlImporterServiceTests
{
    using AutoMoq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Services.Implementation;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ParseToRssChannelList
    {
        private AutoMoqer moqer;
        private OpmlImporterService sut;
        private Mock<IEntityRepository> mockEntityRepository;

        [SetUp]
        public void Setup()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<OpmlImporterService>();
            this.mockEntityRepository = this.moqer.GetMock<IEntityRepository>();
        }

        [Test]
        public void T001()
        {
            // arrange
            // act
            // assert
        }

    }
}