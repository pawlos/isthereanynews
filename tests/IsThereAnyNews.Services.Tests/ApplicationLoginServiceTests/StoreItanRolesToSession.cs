using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using AutoMoq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework.Models.Entities;
using IsThereAnyNews.Services.Implementation;
using Moq;
using Xunit;

namespace IsThereAnyNews.Services.Tests.ApplicationLoginServiceTests
{
    public class StoreItanRolesToSession
    {
        private readonly ILoginService sut;
        private readonly AutoMoqer moqer;
        private readonly Mock<ISessionProvider> mockSessionProvider;
        private readonly Mock<IUserRoleRepository> mockRepositoryUserRoles;
        private readonly Mock<IMapper> mockAutomapper;

        public StoreItanRolesToSession()
        {
            this.moqer = new AutoMoqer();
            this.sut = moqer.Resolve<ApplicationLoginService>();
            this.mockSessionProvider = moqer.GetMock<ISessionProvider>();
            this.mockRepositoryUserRoles = moqer.GetMock<IUserRoleRepository>();
            this.mockAutomapper = moqer.GetMock<IMapper>();
        }

        [Fact]
        public void When_Storing_Roles_To_Session_Must_First_Read_Current_User_From_Session()
        {
            // arrange
            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(0);

            this.mockRepositoryUserRoles
                .Setup(s => s.GetRolesForUser(It.IsAny<long>()))
                .Returns(new List<UserRole>());

            // act
            this.sut.StoreItanRolesToSession();

            // assert
            this.mockSessionProvider
                .Verify(v => v.GetCurrentUserId(),
                Times.Once);
        }

        [Fact]
        public void When_Storing_Roles_To_Session_Must_First_Load_Roles_From_Database()
        {
            // arrange
            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(0);

            this.mockRepositoryUserRoles
                .Setup(s => s.GetRolesForUser(It.IsAny<long>()))
                .Returns(new List<UserRole>());

            // act
            this.sut.StoreItanRolesToSession();

            // assert
            this.mockRepositoryUserRoles
                .Verify(v => v.GetRolesForUser(It.IsAny<long>()),
                Times.Once);
        }

        [Fact]
        public void When_Storing_Roles_To_Session_Must_First_User_Automapper_To_Convert_From_Application_Roles_To_Claims()
        {
            // arrange
            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(0);

            this.mockRepositoryUserRoles
                .Setup(s => s.GetRolesForUser(It.IsAny<long>()))
                .Returns(new List<UserRole>());

            this.mockAutomapper
                .Setup(s => s.Map<List<Claim>>(It.IsAny<List<UserRole>>()))
                .Returns(new List<Claim>());

            // act
            this.sut.StoreItanRolesToSession();

            // assert
            this.mockAutomapper
                .Verify(v => v.Map<List<Claim>>(It.IsAny<List<UserRole>>()),
                Times.Once);
        }

        [Fact]
        public void Converted_Roles_Must_Be_Stored_In_Session()
        {
            // arrange
            this.mockSessionProvider
                .Setup(s => s.GetCurrentUserId())
                .Returns(0);

            this.mockRepositoryUserRoles
                .Setup(s => s.GetRolesForUser(It.IsAny<long>()))
                .Returns(new List<UserRole>());

            this.mockAutomapper
                .Setup(s => s.Map<List<Claim>>(It.IsAny<List<UserRole>>()))
                .Returns(new List<Claim>());

            // act
            this.sut.StoreItanRolesToSession();

            // assert
            this.mockSessionProvider
                .Verify(v => v.SaveClaims(It.IsAny<IEnumerable<Claim>>()),
                Times.Once);
        }
    }
}