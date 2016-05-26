using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.OpmlImporterServiceTests
{
    public class ParseToRssChannelList
    {
        private readonly AutoMoqer moqer;
        private readonly OpmlImporterService sut;
        private readonly Mock<IRssChannelsRepository> mockRssChannelsRepository;
        private readonly Mock<IRssChannelsSubscriptionsRepository> mockRssSubscriptionsRepository;

        public ParseToRssChannelList()
        {
            this.moqer = new AutoMoq.AutoMoqer();
            this.sut = this.moqer.Resolve<OpmlImporterService>();
            this.mockRssChannelsRepository = this.moqer.GetMock<IRssChannelsRepository>();
            this.mockRssSubscriptionsRepository = this.moqer.GetMock<IRssChannelsSubscriptionsRepository>();
        }

        [Fact]
        public void T001()
        {
            // arrange
            // act
            // assert
        }

    }
}