using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;
using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using EntityFrameworkCoreMock;
using Moq;
using EnjOffer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AutoFixture;

namespace UnitTests
{
    public class UsersServiceTest
    {
        private readonly IUsersService _usersService;
        private readonly IFixture _fixture;

        //TO-DO: Avoid the Service Locator Anti-Pattern here. Use Dependency Injection
        public UsersServiceTest()
        {
            /*var usersInitialData = new List<Users>();
            DbContextMock<EnjOfferDbContext> dbContextMock =
                new DbContextMock<EnjOfferDbContext>(new DbContextOptionsBuilder<EnjOfferDbContext>().Options);

            EnjOfferDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Users);*/
            
            _usersService = new UsersService();
            _fixture = new Fixture();
        }

        #region AddUser

        [Fact]
        public void AddUser_NullUser()
        {
            //Arrange
            UserAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _usersService.AddUser(request);
            });
        }

        [Fact]
        public void AddUser_EmailIsNull()
        {
            //Arrange
            UserAddRequest? request = _fixture.Build<UserAddRequest>().With(temp => temp.Email, null as string).Create();

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _usersService.AddUser(request);
            });
        }

        [Fact]
        public void AddUser_DuplicateValues()
        {
            //Arrange
            UserAddRequest? request1 = new UserAddRequest()
            {
                Email = "example@example.com",
                Password = "password",
                Role = EnjOffer.Core.Enums.UserRole.Admin
            };
            UserAddRequest? request2 = new UserAddRequest()
            {
                Email = "example@example.com",
                Password = "password",
                Role = EnjOffer.Core.Enums.UserRole.Admin
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _usersService.AddUser(request1);
                _usersService.AddUser(request2);
            });
        }

        [Fact]
        public void AddUser_ProperUserDetails()
        {
            //Arrange
            UserAddRequest? userAddRequest = _fixture.Create<UserAddRequest>();

            //Act
            UserResponse user_response_from_add = _usersService.AddUser(userAddRequest);
            List<UserResponse> users_list = _usersService.GetAllUsers();

            //Assert
            Assert.True(user_response_from_add.UserId != Guid.Empty);
            Assert.Contains(user_response_from_add, users_list);
        }

        #endregion

        #region GetAllUsers

        [Fact]
        public void GetAllUsers_EmptyList()
        {
            //Act
            List<UserResponse> actual_users_response_list = _usersService.GetAllUsers();

            //Assert
            Assert.Empty(actual_users_response_list);
        }

        [Fact]
        public void GetAllDefaultWords_AddFewDefaultWords()
        {
            //Arrange
            List<UserAddRequest> users_request_list = new List<UserAddRequest>()
            {
                _fixture.Create<UserAddRequest>(),
                _fixture.Create<UserAddRequest>()
        };

            //Act
            List<UserResponse> users_list_from_add = new List<UserResponse>();
            foreach (UserAddRequest user_request in users_request_list)
            {
                users_list_from_add.Add(_usersService.AddUser(user_request));
            }

            List<UserResponse> actualUsersResponseList = _usersService.GetAllUsers();

            //Assert
            foreach (UserResponse expected_user in users_list_from_add)
            {
                Assert.Contains(expected_user, actualUsersResponseList);
            }
        }

        #endregion

        #region GetUserById

        [Fact]
        public void GetUserById_NullUserId()
        {
            //Arrange
            Guid? userId = null;

            //Act
            UserResponse? user_response_from_get_method =
                _usersService.GetUserById(userId);

            //Assert
            Assert.Null(user_response_from_get_method);
        }

        [Fact]
        public void GetUserById_ValidUserId()
        {
            //Arrange
            UserAddRequest? user_add_request = _fixture.Create<UserAddRequest>();
            UserResponse user_response_from_add = _usersService.AddUser(user_add_request);

            //Act
            UserResponse? duser_from_get = _usersService.GetUserById
                (user_response_from_add.UserId);

            //Assert
            Assert.Equal(user_response_from_add, duser_from_get);
        }

        #endregion

        #region DeleteUser

        [Fact]
        public void DeleteUser_ValidId()
        {
            //Arrange
            UserAddRequest user_add_request = _fixture.Create<UserAddRequest>();
            UserResponse user_response_from_add = _usersService.AddUser(user_add_request);

            //Act
            bool isDeleted = _usersService.DeleteUser(user_response_from_add.UserId);

            //Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeleteUser_InvalidId()
        {
            //Act
            bool isDeleted = _usersService.DeleteUser(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }

        #endregion
    }
}
