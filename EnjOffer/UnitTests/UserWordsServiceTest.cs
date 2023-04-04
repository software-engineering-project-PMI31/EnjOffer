using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class UserWordsServiceTest
    {
        private readonly IUserWordsService _userWordsService;
        private readonly IUsersService _usersService;

        //TO-DO: Avoid the Service Locator Anti-Pattern here. Use Dependency Injection
        public UserWordsServiceTest()
        {
            _userWordsService = new UserWordsService();
            _usersService = new UsersService();
        }

        #region AddUserWord

        [Fact]
        public void AddUserWord_NullUserWord()
        {
            //Arrange
            UserWordsAddRequest? userWordAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _userWordsService.AddUserWord(userWordAddRequest);
            });
        }

        [Fact]
        public void AddUserWord_ProperDetails()
        {
            //Arrange
            UserWordsAddRequest? userWordAddRequest = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = Guid.NewGuid()
            };

            //Act
            UserWordsResponse userWord_response_from_add =
                _userWordsService.AddUserWord(userWordAddRequest);
            List<UserWordsResponse> userWords_list = _userWordsService.GetAllUserWords();

            //Assert
            Assert.True(userWord_response_from_add.UserWordId != Guid.Empty);
            Assert.Contains(userWord_response_from_add, userWords_list);
        }

        [Fact]
        public void AddUsertWord_DuplicateValues()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            UserWordsAddRequest? request1 = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = guid
                
            };
            UserWordsAddRequest? request2 = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = guid
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _userWordsService.AddUserWord(request1);
                _userWordsService.AddUserWord(request2);
            });
        }

        #endregion

        #region GetPriority

        [Fact]
        public void GetPriority_NullDate()
        {
            //Arrange
            UserWordsAddRequest userWordRequest = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = Guid.NewGuid()
            };
            
            //Act
            UserWordsResponse userWord_from_add = _userWordsService.AddUserWord(userWordRequest);

            //Assert
            Assert.Equal(1, userWord_from_add.Priority);
        }

        #endregion

        #region GetUserWordById

        [Fact]
        public void GetUserWordById_NullId()
        {
            //Arrange
            Guid? userWordId = null;

            //Act
            UserWordsResponse? userWord_response_from_get = _userWordsService.GetUserWordById(userWordId);

            //Assert
            Assert.Null(userWord_response_from_get);
        }

        [Fact]
        public void GetUserWordById_ProperId()
        {
            //Arrange
            UserAddRequest user_request = new UserAddRequest()
            {
                Email = "example@example.com",
                Password = "password",
                Role = EnjOffer.Core.Enums.UserRole.Admin
            };
            UserResponse user_response = _usersService.AddUser(user_request);

            //Act
            UserWordsAddRequest userWord_request = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = user_response.UserId
            };
            UserWordsResponse userWord_response_from_add = _userWordsService.AddUserWord(userWord_request);
            UserWordsResponse? userWord_response_from_get = _userWordsService.GetUserWordById(userWord_response_from_add.UserWordId);

            //Assert
            Assert.Equal(userWord_response_from_add, userWord_response_from_get);
        }


        #endregion

        #region GetAllUserWords

        [Fact]
        public void GetAllUsertWords_EmptyList()
        {
            //Act
            List<UserWordsResponse> actual_userWords_response_list = _userWordsService.GetAllUserWords();

            //Assert
            Assert.Empty(actual_userWords_response_list);
        }

        [Fact]
        public void GetAllUserWords_AddFewUserWords()
        {
            //Arrange
            UserAddRequest userAdd_request1 = new UserAddRequest()
            {
                Email = "example@example.com",
                Password = "password",
                Role = EnjOffer.Core.Enums.UserRole.Admin
            };
            UserAddRequest userAdd_request2 = new UserAddRequest()
            {
                Email = "example1@example.com",
                Password = "password",
                Role = EnjOffer.Core.Enums.UserRole.Admin
            };

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);
            UserResponse user_response2 = _usersService.AddUser(userAdd_request2);


            UserWordsAddRequest userWord_request1 = new UserWordsAddRequest() 
            { 
                Word = "Something",
                WordTranslation = "Щось",
                UserId = user_response1.UserId
            };
            UserWordsAddRequest userWord_request2 = new UserWordsAddRequest() 
            {
                Word = "Someone",
                WordTranslation = "Хтось",
                UserId = user_response2.UserId
            };
            UserWordsAddRequest userWord_request3 = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = user_response2.UserId
            };

            List<UserWordsAddRequest> userWords_requests = new List<UserWordsAddRequest>()
            {
                userWord_request1, userWord_request2, userWord_request3
            };

            List<UserWordsResponse> userWords_response_list_from_add = new List<UserWordsResponse>();

            foreach (UserWordsAddRequest userWord in userWords_requests)
            {
                UserWordsResponse userWord_response = _userWordsService.AddUserWord(userWord);
                userWords_response_list_from_add.Add(userWord_response);
            }

            //Act
            List<UserWordsResponse> userWords_from_get = _userWordsService.GetAllUserWords();

            //Assert
            foreach (UserWordsResponse userWord_response_from_add in userWords_response_list_from_add)
            {
                Assert.Contains(userWord_response_from_add, userWords_from_get);
            }
        }

        #endregion

        #region DeleteDefaultWord

        [Fact]
        public void DeleteUserWord_ValidId()
        {
            //Arrange
            UserAddRequest user_add_request = new UserAddRequest()
            {
                Email = "example@example.com",
                Password = "Password",
                Role = EnjOffer.Core.Enums.UserRole.Admin
            };
            UserResponse user_response = _usersService.AddUser(user_add_request);

            UserWordsAddRequest userWord_add_request = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = user_response.UserId
            };
            UserWordsResponse userWord_response_from_add = _userWordsService.AddUserWord(userWord_add_request);

            //Act
            bool isDeleted = _userWordsService.DeleteUserWord(userWord_response_from_add.UserWordId);

            //Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeleteUserWord_InvalidId()
        {
            //Act
            bool isDeleted = _userWordsService.DeleteUserWord(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }

        #endregion
    }
}
