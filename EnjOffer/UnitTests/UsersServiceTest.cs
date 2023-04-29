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
using EnjOffer.Core.Domain.RepositoryContracts;

namespace UnitTests
{
    public class UsersServiceTest
    {
        private readonly Mock<IUsersRepository> _usersRepositoryMock;
        private readonly IUsersRepository _usersRepository;
        private readonly IUsersService _usersService;

        private readonly Mock<IDefaultWordsRepository> _defaultWordsRepositoryMock;
        private readonly IDefaultWordsRepository _defaultWordsRepository;

        private readonly Mock<IUsersDefaultWordsRepository> _usersDefaultWordsRepositoryMock;
        private readonly IUsersDefaultWordsRepository _usersDefaultWordsRepository;

        private readonly IFixture _fixture;

        //TO-DO: Avoid the Service Locator Anti-Pattern here. Use Dependency Injection
        public UsersServiceTest()
        {
            _fixture = new Fixture();

            _usersRepositoryMock = new Mock<IUsersRepository>();
            _usersRepository = _usersRepositoryMock.Object;

            _defaultWordsRepositoryMock = new Mock<IDefaultWordsRepository>();
            _defaultWordsRepository = _defaultWordsRepositoryMock.Object;

            _usersDefaultWordsRepositoryMock = new Mock<IUsersDefaultWordsRepository>();
            _usersDefaultWordsRepository = _usersDefaultWordsRepositoryMock.Object;

            /*var usersInitialData = new List<Users>();
            DbContextMock<EnjOfferDbContext> dbContextMock =
                new DbContextMock<EnjOfferDbContext>(new DbContextOptionsBuilder<EnjOfferDbContext>().Options);

            EnjOfferDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Users, usersInitialData);*/

            _usersService = new UsersService(_usersRepository, _defaultWordsRepository, _usersDefaultWordsRepository);
        }

        #region AddUser

        [Fact]
        public void AddUser_NullUser()
        {
            //Arrange
            UserAddRequest? request = null;

            Users user = _fixture.Build<Users>()
                .With(temp => temp.DefaultWords, null as ICollection<DefaultWords>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .With(temp => temp.UserStatistics, null as ICollection<UserStatistics>)
                .With(temp => temp.UserWords, null as ICollection<UserWords>)
                .Create();

            _usersRepositoryMock.Setup(temp => temp.AddUser(It.IsAny<Users>()))
                .Returns(user);

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

            Users user = _fixture.Build<Users>()
                .With(temp => temp.DefaultWords, null as ICollection<DefaultWords>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .With(temp => temp.UserStatistics, null as ICollection<UserStatistics>)
                .With(temp => temp.UserWords, null as ICollection<UserWords>)
                .Create();

            _usersRepositoryMock.Setup(temp => temp.AddUser(It.IsAny<Users>()))
                .Returns(user);

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

            Users first_user = request1.ToUser();

            /*_usersRepositoryMock.SetupSequence(temp => temp.AddUser(It.IsAny<Users>()))
                .Returns(first_user)
                .Throws<ArgumentException>();*/

            _usersRepositoryMock.SetupSequence(temp => temp.AddUser(It.IsAny<Users>()))
                .Throws(new ArgumentException("User with same email already exists"))
                .Returns(first_user);


            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _usersService.AddUser(request2);
            });
        }

        [Fact]
        public void AddUser_ProperUserDetails()
        {
            //Arrange
            UserAddRequest? userAddRequest = _fixture.Create<UserAddRequest>();
            Users user = userAddRequest.ToUser();
            UserResponse user_response = user.ToUserResponse();

            _usersRepositoryMock.Setup(temp => temp.AddUser(It.IsAny<Users>()))
                .Returns(user);

            //Act
            UserResponse user_response_from_add = _usersService.AddUser(userAddRequest);

            user.UserId = user_response_from_add.UserId;
            user_response.UserId = user_response_from_add.UserId;

            //Assert
            Assert.True(user_response_from_add.UserId != Guid.Empty);
            Assert.Equal(user_response_from_add, user_response);
        }

        #endregion

        #region GetAllUsers

        [Fact]
        public void GetAllUsers_EmptyList()
        {
            //Arrange
            List<Users> users = new List<Users>();
            _usersRepositoryMock.Setup(temp => temp.GetAllUsers()).Returns(users);

            //Act
            List<UserResponse> actual_users_response_list = _usersService.GetAllUsers();

            //Assert
            Assert.Empty(actual_users_response_list);
        }

        [Fact]
        public void GetAllUsers_AddFewUsers()
        {
            //Arrange
            List<Users> users_list = new List<Users>()
            {
                _fixture.Build<Users>()
                .With(temp => temp.DefaultWords, null as ICollection<DefaultWords>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .With(temp => temp.UserStatistics, null as ICollection<UserStatistics>)
                .With(temp => temp.UserWords, null as ICollection<UserWords>)
                .Create(),

                _fixture.Build<Users>()
                .With(temp => temp.DefaultWords, null as ICollection<DefaultWords>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .With(temp => temp.UserStatistics, null as ICollection<UserStatistics>)
                .With(temp => temp.UserWords, null as ICollection<UserWords>)
                .Create()
            };

            List<UserResponse> users_response_list = users_list.Select(temp => temp.ToUserResponse()).ToList();

            _usersRepositoryMock.Setup(temp => temp.GetAllUsers()).Returns(users_list);

            //Act
            List<UserResponse> actualUsersResponseList = _usersService.GetAllUsers();

            //Assert
            Assert.Equal(users_response_list, actualUsersResponseList);
        }

        #endregion

        #region GetUserById

        [Fact]
        public void GetUserById_NullUserId()
        {
            //Arrange
            Guid? userId = null;
            _usersRepositoryMock.Setup(temp => temp.GetUserById(It.IsAny<Guid>())).Returns(null as Users);

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
            Users user = _fixture.Build<Users>()
                .With(temp => temp.DefaultWords, null as ICollection<DefaultWords>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .With(temp => temp.UserStatistics, null as ICollection<UserStatistics>)
                .With(temp => temp.UserWords, null as ICollection<UserWords>)
                .Create();

            UserResponse user_response = user.ToUserResponse();

            _usersRepositoryMock.Setup(temp => temp.GetUserById(It.IsAny<Guid>())).Returns(user);

            //Act
            UserResponse? user_from_get = _usersService.GetUserById(user.UserId);

            //Assert
            Assert.Equal(user_response, user_from_get);
        }

        #endregion

        #region DeleteUser

        [Fact]
        public void DeleteUser_ValidId()
        {
            //Arrange
            Users user = _fixture.Build<Users>()
                .With(temp => temp.DefaultWords, null as ICollection<DefaultWords>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .With(temp => temp.UserStatistics, null as ICollection<UserStatistics>)
                .With(temp => temp.UserWords, null as ICollection<UserWords>)
                .Create();

            _usersRepositoryMock.Setup(temp => temp.DeleteUser(It.IsAny<Guid>())).Returns(true);
            _usersRepositoryMock.Setup(temp => temp.GetUserById(It.IsAny<Guid>())).Returns(user);

            //Act
            bool isDeleted = _usersService.DeleteUser(user.UserId);

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
