using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;

namespace UnitTests
{
    public class UserWordsServiceTest
    {
        private readonly IUserWordsService _userWordsService;
        private readonly IUsersService _usersService;
        private readonly IFixture _fixture;


        //TO-DO: Avoid the Service Locator Anti-Pattern here. Use Dependency Injection
        public UserWordsServiceTest()
        {
            _userWordsService = new UserWordsService();
            _usersService = new UsersService();
            _fixture = new Fixture();
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
            UserWordsAddRequest? userWordAddRequest = _fixture.Create<UserWordsAddRequest>();

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
            UserWordsAddRequest userWordRequest = _fixture.Create<UserWordsAddRequest>();
            //Act
            UserWordsResponse userWord_from_add = _userWordsService.AddUserWord(userWordRequest);

            //Assert
            Assert.Equal(1, userWord_from_add.Priority);
        }

        [Fact]
        public void GetPriority_AfterUpdate()
        {
            //Arrange
            DateTime yesterday = DateTime.Now.Date.AddDays(-1);
            int correctEnteredCount = 3, incorrectEnteredCount = 3;
            double expected = 0.49992;

            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();
            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);


            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsResponse userWord_response1 = _userWordsService.AddUserWord(userWord_request1);

            UserWordsUpdateRequest userWord_update_request = userWord_response1.ToUserWordsUpdateRequest();
            userWord_update_request.LastTimeEntered = yesterday;
            userWord_update_request.CorrectEnteredCount = correctEnteredCount;
            userWord_update_request.IncorrectEnteredCount = incorrectEnteredCount;

            //Act
            UserWordsResponse userWord_from_update = _userWordsService.UpdateUserWord(userWord_update_request);

            //Assert
            Assert.Equal(expected, Math.Round(userWord_from_update.Priority, 5));
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
            UserAddRequest user_request = _fixture.Create<UserAddRequest>();
            UserResponse user_response = _usersService.AddUser(user_request);

            //Act
            UserWordsAddRequest userWord_request = _fixture.Create<UserWordsAddRequest>();
            UserWordsResponse userWord_response_from_add = _userWordsService.AddUserWord(userWord_request);
            UserWordsResponse? userWord_response_from_get = _userWordsService.GetUserWordById(userWord_response_from_add.UserWordId);

            //Assert
            Assert.Equal(userWord_response_from_add, userWord_response_from_get);
        }


        #endregion

        #region GetUserWordsByDate

        [Fact]
        public void GetUserWordsByDate_NullDate()
        {
            //Arrange
            DateTime? date = null;

            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();
            UserAddRequest userAdd_request2 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);
            UserResponse user_response2 = _usersService.AddUser(userAdd_request2);


            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsAddRequest userWord_request2 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

            UserWordsAddRequest userWord_request3 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

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
            List<UserWordsResponse> userWords_from_get = _userWordsService.GetUserWordsByDate(date);

            //Assert
            foreach (UserWordsResponse userWord_response_from_add in userWords_response_list_from_add)
            {
                Assert.Contains(userWord_response_from_add, userWords_from_get);
            }
        }

        [Fact]
        public void GetUserWordsByDate_ProperDate()
        {
            //Arrange
            DateTime dateNow = DateTime.Now.Date;
            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();
            UserAddRequest userAdd_request2 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);
            UserResponse user_response2 = _usersService.AddUser(userAdd_request2);


            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsAddRequest userWord_request2 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

            UserWordsAddRequest userWord_request3 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

            List<UserWordsAddRequest> userWords_requests = new List<UserWordsAddRequest>()
            {
                userWord_request1, userWord_request2, userWord_request3
            };

            List<UserWordsResponse> userWords_response_list_from_add = new List<UserWordsResponse>();

            foreach (UserWordsAddRequest userWord in userWords_requests)
            {
                UserWordsResponse userWord_response = _userWordsService.AddUserWord(userWord);

                UserWordsUpdateRequest userWord_update_request = userWord_response.ToUserWordsUpdateRequest();
                userWord_update_request.LastTimeEntered = dateNow;
                UserWordsResponse userWord_response_from_update = _userWordsService.UpdateUserWord(userWord_update_request);

                userWords_response_list_from_add.Add(userWord_response_from_update);
            }

            //Act
            List<UserWordsResponse> userWords_from_get = _userWordsService.GetUserWordsByDate(dateNow);

            List<Guid> userWordsIds_from_get = new List<Guid>();
            List<Guid> userWordsIds_from_add = new List<Guid>();

            userWordsIds_from_get.AddRange(userWords_from_get.Select(temp => temp.UserWordId));
            userWordsIds_from_add.AddRange(userWords_response_list_from_add.Select(temp => temp.UserWordId));

            //Assert
            foreach (Guid userWordsId_response in userWordsIds_from_add)
            {
                Assert.Contains(userWordsId_response, userWordsIds_from_get);
            }
        }

        #endregion

        #region GetUserWordsSortedByPriority
        [Fact]
        public void GetUserWordsSortedByPriority_DefaultPriority()
        {
            //Arrange
            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();
            UserAddRequest userAdd_request2 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);
            UserResponse user_response2 = _usersService.AddUser(userAdd_request2);


            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsAddRequest userWord_request2 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

            UserWordsAddRequest userWord_request3 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

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

            List<UserWordsResponse> allUserWords = _userWordsService.GetAllUserWords();

            //Act
            List<UserWordsResponse> userWords_sorted = _userWordsService.GetUserWordsSortedByPriority(allUserWords);
            userWords_response_list_from_add = userWords_response_list_from_add.OrderByDescending(temp => temp.Priority).ToList();

            //Assert
            for (int i = 0; i < userWords_response_list_from_add.Count; i++)
            {
                Assert.Equal(userWords_response_list_from_add[i], userWords_sorted[i]);
            }
        }

        [Fact]
        public void GetUserWordsSortedByPriority_UpdatedPriority()
        {
            //Arrange
            DateTime dateNow = DateTime.Now.Date;
            DateTime dateWeekAgo = DateTime.Now.Date.AddDays(-7);
            DateTime dateMonthAgo = DateTime.Now.Date.AddMonths(-1);

            int correctEnteredCountForNow = 1, incorrectEnteredCountForNow = 3;
            int correctEnteredCountForWeekAgo = 14, incorrectEnteredCountForWeekAgo = 4;
            int correctEnteredCountForMonthAgo = 16, incorrectEnteredCountForMonthAgo = 43;

            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();
            UserAddRequest userAdd_request2 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);
            UserResponse user_response2 = _usersService.AddUser(userAdd_request2);


            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsAddRequest userWord_request2 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

            UserWordsAddRequest userWord_request3 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

            List<UserWordsAddRequest> userWords_requests = new List<UserWordsAddRequest>()
            {
                userWord_request1, userWord_request2, userWord_request3
            };

            List<UserWordsResponse> userWords_response_list_from_add = new List<UserWordsResponse>();

            for (int i = 0; i < userWords_requests.Count; i++)
            {
                UserWordsResponse userWord_response = _userWordsService.AddUserWord(userWords_requests[i]);
                UserWordsUpdateRequest userWord_update_request = userWord_response.ToUserWordsUpdateRequest();
                switch (i)
                {
                    case 0:
                        userWord_update_request.LastTimeEntered = dateNow;
                        userWord_update_request.CorrectEnteredCount = correctEnteredCountForNow;
                        userWord_update_request.IncorrectEnteredCount = incorrectEnteredCountForNow;

                        break;
                    case 1:
                        userWord_update_request.LastTimeEntered = dateWeekAgo;
                        userWord_update_request.CorrectEnteredCount = correctEnteredCountForWeekAgo;
                        userWord_update_request.IncorrectEnteredCount = incorrectEnteredCountForWeekAgo;

                        break;
                    case 2:
                        userWord_update_request.LastTimeEntered = dateMonthAgo;
                        userWord_update_request.CorrectEnteredCount = correctEnteredCountForMonthAgo;
                        userWord_update_request.IncorrectEnteredCount = incorrectEnteredCountForMonthAgo;

                        break;
                }

                UserWordsResponse userWord_response_from_update = _userWordsService.UpdateUserWord(userWord_update_request);
                userWords_response_list_from_add.Add(userWord_response_from_update);
            }

            List<UserWordsResponse> allUserWords = _userWordsService.GetAllUserWords();

            //Act
            List<UserWordsResponse> userWords_sorted = _userWordsService.GetUserWordsSortedByPriority(allUserWords);
            userWords_response_list_from_add = userWords_response_list_from_add.OrderByDescending(temp => temp.Priority).ToList();

            //Assert
            for (int i = 0; i < userWords_response_list_from_add.Count; i++)
            {
                Assert.Equal(userWords_response_list_from_add[i].UserWordId, userWords_sorted[i].UserWordId);
                Assert.Equal(Math.Round(userWords_response_list_from_add[i].Priority, 6), Math.Round(userWords_sorted[i].Priority, 6));
            }
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
            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();
            UserAddRequest userAdd_request2 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);
            UserResponse user_response2 = _usersService.AddUser(userAdd_request2);


            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsAddRequest userWord_request2 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

            UserWordsAddRequest userWord_request3 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response2.UserId).Create();

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

        #region UpdateUserWord

        [Fact]
        public void UpdateUserWord_NullUserWord()
        {
            //Arrange
            UserWordsUpdateRequest? userWord_update_request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _userWordsService.UpdateUserWord(userWord_update_request);
            });
        }

        [Fact]
        public void UpdateUserWord_InvalidUserWordId()
        {
            //Arrange
            UserWordsUpdateRequest? userWord_update_request = new UserWordsUpdateRequest()
            {
                UserWordId = Guid.NewGuid()
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _userWordsService.UpdateUserWord(userWord_update_request);
            });
        }

        [Fact]
        public void UpdateUserWord_IncreaseBothCorrectAndIncorrect()
        {
            //Arrange
            UserWordsUpdateRequest userWord_update_request = new UserWordsUpdateRequest()
            {
                IsIncreaseCorrectEnteredCount = true,
                IsIncreaseIncorrectEnteredCount = true
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _userWordsService.UpdateUserWord(userWord_update_request);
            });
        }

        [Fact]
        public void UpdateUserWord_IncreaseCorrectEnteredCount()
        {
            //Arrange
            int expected = 1;
            int expected1 = 0;
            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);

            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsResponse userWord_response_from_add = _userWordsService.AddUserWord(userWord_request1);

            UserWordsUpdateRequest userWord_update_request = userWord_response_from_add.ToUserWordsUpdateRequest();

            userWord_update_request.IsIncreaseCorrectEnteredCount = true;

            //Act
            UserWordsResponse userWords_response_from_update = _userWordsService.UpdateUserWord(userWord_update_request);

            //Assert
            Assert.Equal(expected, userWords_response_from_update.CorrectEnteredCount);
            Assert.Equal(expected1, userWords_response_from_update.IncorrectEnteredCount);
        }

        [Fact]
        public void UpdateUserWord_IncreaseCorrectEnteredCountTwice()
        {
            //Arrange
            int expected = 2;
            int expected1 = 0;
            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);

            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsResponse userWord_response_from_add = _userWordsService.AddUserWord(userWord_request1);
            UserWordsUpdateRequest userWord_update_first_request = userWord_response_from_add.ToUserWordsUpdateRequest();
            userWord_update_first_request.IsIncreaseCorrectEnteredCount = true;
            UserWordsResponse userWords_response_from_first_update = _userWordsService.UpdateUserWord(userWord_update_first_request);

            UserWordsUpdateRequest userWord_update_second_request = userWords_response_from_first_update.ToUserWordsUpdateRequest();
            userWord_update_second_request.IsIncreaseCorrectEnteredCount = true;

            //Act
            UserWordsResponse userWords_response_from_second_update = _userWordsService.UpdateUserWord(userWord_update_second_request);

            //Assert
            Assert.Equal(expected, userWords_response_from_second_update.CorrectEnteredCount);
            Assert.Equal(expected1, userWords_response_from_second_update.IncorrectEnteredCount);
        }

        [Fact]
        public void UpdateUserWord_IncreaseIncorrectEnteredCount()
        {
            //Arrange
            int expected = 1;
            int expected1 = 0;
            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);

            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsResponse userWord_response_from_add = _userWordsService.AddUserWord(userWord_request1);

            UserWordsUpdateRequest userWord_update_request = userWord_response_from_add.ToUserWordsUpdateRequest();

            userWord_update_request.IsIncreaseIncorrectEnteredCount = true;

            //Act
            UserWordsResponse userWords_response_from_update = _userWordsService.UpdateUserWord(userWord_update_request);

            //Assert
            Assert.Equal(expected, userWords_response_from_update.IncorrectEnteredCount);
            Assert.Equal(expected1, userWords_response_from_update.CorrectEnteredCount);
        }

        [Fact]
        public void UpdateUserWord_LastTimeEnteredIsNull()
        {
            //Arrange
            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);

            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsResponse userWord_response_from_add = _userWordsService.AddUserWord(userWord_request1);

            UserWordsUpdateRequest userWord_update_request = userWord_response_from_add.ToUserWordsUpdateRequest();

            userWord_update_request.LastTimeEntered = null;

            //Act
            UserWordsResponse userWords_response_from_update = _userWordsService.UpdateUserWord(userWord_update_request);

            //Assert
            Assert.Null(userWords_response_from_update.LastTimeEntered);
        }

        [Fact]
        public void UpdateUserWord_UpdateLastTimeEntered()
        {
            //Arrange
            DateTime expectedDate = DateTime.Now;
            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);

            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsResponse userWord_response_from_add = _userWordsService.AddUserWord(userWord_request1);

            UserWordsUpdateRequest userWord_update_request = userWord_response_from_add.ToUserWordsUpdateRequest();

            userWord_update_request.LastTimeEntered = expectedDate;

            //Act
            UserWordsResponse userWords_response_from_update = _userWordsService.UpdateUserWord(userWord_update_request);

            //Assert
            Assert.Equal(expectedDate, userWords_response_from_update.LastTimeEntered);
        }

        [Fact]
        public void UpdateUserWord_UpdateLastTimeEnteredTwiceAndSetNull()
        {
            //Arrange
            DateTime expectedDate = DateTime.Now;
            UserAddRequest userAdd_request1 = _fixture.Create<UserAddRequest>();

            UserResponse user_response1 = _usersService.AddUser(userAdd_request1);

            UserWordsAddRequest userWord_request1 = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response1.UserId).Create();

            UserWordsResponse userWord_response_from_add = _userWordsService.AddUserWord(userWord_request1);

            UserWordsUpdateRequest userWord_update_first_request = userWord_response_from_add.ToUserWordsUpdateRequest();
            userWord_update_first_request.LastTimeEntered = expectedDate;
            UserWordsResponse userWords_response_from_update = _userWordsService.UpdateUserWord(userWord_update_first_request);

            UserWordsUpdateRequest userWord_update_second_request = userWords_response_from_update.ToUserWordsUpdateRequest();
            userWord_update_second_request.LastTimeEntered = null;

            //Act
            UserWordsResponse userWords_second_response_from_update = _userWordsService.UpdateUserWord(userWord_update_second_request);

            //Assert
            Assert.Equal(expectedDate, userWords_second_response_from_update.LastTimeEntered);
        }

        #endregion

        #region DeleteDefaultWord

        [Fact]
        public void DeleteUserWord_ValidId()
        {
            //Arrange
            UserAddRequest user_add_request = _fixture.Create<UserAddRequest>();
            UserResponse user_response = _usersService.AddUser(user_add_request);

            UserWordsAddRequest userWord_add_request = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.UserId, user_response.UserId).Create();

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