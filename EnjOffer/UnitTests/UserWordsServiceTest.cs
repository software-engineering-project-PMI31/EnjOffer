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
using Moq;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;

namespace UnitTests
{
    public class UserWordsServiceTest
    {
        private readonly Mock<IUserWordsRepository> _userWordsRepositoryMock;
        private readonly IUserWordsRepository _userWordsRepository;
        private readonly IUserWordsService _userWordsService;

        private readonly Mock<IUserStatisticsRepository> _userStatisticsRepositoryMock;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly IUserStatisticsService _userStatisticsService;
        private readonly IFixture _fixture;

        public UserWordsServiceTest()
        {
            _fixture = new Fixture();

            _userWordsRepositoryMock = new Mock<IUserWordsRepository>();
            _userWordsRepository = _userWordsRepositoryMock.Object;

            _userStatisticsRepositoryMock = new Mock<IUserStatisticsRepository>();
            _userStatisticsRepository = _userStatisticsRepositoryMock.Object;

            _userWordsService = new UserWordsService(_userWordsRepository);
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
        public void AddUserWord_NullWord()
        {
            //Arrange
            UserWordsAddRequest? userWord_add_request = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.Word, null as string)
                .Create();

            UserWords userWord = userWord_add_request.ToUserWords();

            _userWordsRepositoryMock.Setup(temp => temp.AddUserWord(It.IsAny<UserWords>())).Returns(userWord);

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _userWordsService.AddUserWord(userWord_add_request);
            });
        }

        [Fact]
        public void AddUserWord_NullWordTranslation()
        {
            //Arrange
            UserWordsAddRequest? userWord_add_request = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.WordTranslation, null as string)
                .Create();

            UserWords userWord = userWord_add_request.ToUserWords();

            _userWordsRepositoryMock.Setup(temp => temp.AddUserWord(It.IsAny<UserWords>())).Returns(userWord);

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _userWordsService.AddUserWord(userWord_add_request);
            });
        }

        [Fact]
        public void AddUserWord_NullWordAndWordTranslation()
        {
            //Arrange
            UserWordsAddRequest? userWord_add_request = _fixture.Build<UserWordsAddRequest>()
                .With(temp => temp.Word, null as string)
                .With(temp => temp.WordTranslation, null as string)
                .Create();

            UserWords userWord = userWord_add_request.ToUserWords();

            _userWordsRepositoryMock.Setup(temp => temp.AddUserWord(It.IsAny<UserWords>())).Returns(userWord);

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _userWordsService.AddUserWord(userWord_add_request);
            });
        }

        [Fact]
        public void AddUserWord_ProperDetails()
        {
            //Arrange
            UserWordsAddRequest? userWord_add_request = _fixture.Create<UserWordsAddRequest>();
            UserWords userWord = userWord_add_request.ToUserWords();

            UserWordsResponse userWord_response_expected = userWord.ToUserWordsResponse(_userWordsService);
            _userWordsRepositoryMock.Setup(temp => temp.AddUserWord(It.IsAny<UserWords>())).Returns(userWord);


            //Act
            UserWordsResponse userWord_response_from_add =
                _userWordsService.AddUserWord(userWord_add_request);

            userWord_response_expected.UserWordId = userWord_response_from_add.UserWordId;

            //Assert
            Assert.True(userWord_response_from_add.UserWordId != Guid.Empty);
            Assert.Equal(userWord_response_expected, userWord_response_from_add);
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

            UserWords fisrt_userWord = request1.ToUserWords();

            _userWordsRepositoryMock.SetupSequence(temp => temp.AddUserWord(It.IsAny<UserWords>()))
            .Throws(new ArgumentException("The defaultWord with such word and translation already exist"))
            .Returns(fisrt_userWord);

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _userWordsService.AddUserWord(request2);
            });
        }

        #endregion

        #region GetPriority

        //TO-DO: Consider making the test as integrational
        [Fact]
        public void GetPriority_NullDate()
        {
            //Arrange
            UserWordsAddRequest userWordRequest = _fixture.Create<UserWordsAddRequest>();
            UserWords userWord = userWordRequest.ToUserWords();

            _userWordsRepositoryMock.Setup(temp => temp.AddUserWord(It.IsAny<UserWords>())).Returns(userWord);

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
            UserWords userWord = _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-3))
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users)
                .Create();

            UserWordsResponse userWord_response_expected = userWord.ToUserWordsResponse(_userWordsService);

            _userWordsRepositoryMock.Setup(temp => temp.GetUserWordById(It.IsAny<Guid>())).Returns(userWord);

            //Act
            UserWordsResponse? userWord_from_get = _userWordsService.GetUserWordById(userWord.UserId);
            
            //Assert
            Assert.Equal(userWord_response_expected, userWord_from_get);
        }


        #endregion

        #region GetUserWordsByDate

        [Fact]
        public void GetUserWordsByDate_NullDate()
        {
            //Arrange
            DateTime? date = null;

            List<UserWords> userWords = new List<UserWords>()
            {
                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, (DateTime?)null)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create(),

                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, (DateTime?)null)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create()
            };

            _userWordsRepositoryMock.Setup(temp => temp.GetUserWordsByDate(It.IsAny<DateTime?>())).Returns(userWords);

            List<UserWordsResponse> userWords_response_list_expected = userWords.Select(
                temp => temp.ToUserWordsResponse(_userWordsService)).ToList();

            //Act
            List<UserWordsResponse> userWords_from_get = _userWordsService.GetUserWordsByDate(date);

            //Assert
            Assert.Equal(userWords_response_list_expected, userWords_from_get);
        }

        [Fact]
        public void GetUserWordsByDate_ProperDate()
        {
            //Arrange
            DateTime dateNow = DateTime.Now.Date;

            List<UserWords> userWords = new List<UserWords>()
            {
                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, dateNow)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create(),

                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, dateNow)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create()
            };

            _userWordsRepositoryMock.Setup(temp => temp.GetUserWordsByDate(It.IsAny<DateTime?>())).Returns(userWords);

            List<UserWordsResponse> userWords_response_list_expected = userWords.Select(
                temp => temp.ToUserWordsResponse(_userWordsService)).ToList();

            //Act
            List<UserWordsResponse> userWords_from_get = _userWordsService.GetUserWordsByDate(dateNow);

            //Assert
            Assert.Equal(userWords_response_list_expected, userWords_from_get);
        }

        #endregion

        #region GetUserWordsSortedByPriority
        [Fact]
        public void GetUserWordsSortedByPriority_DefaultPriority()
        {
            //Arrange
            int expectedPriority = 1;
            List<UserWords> userWords = new List<UserWords>()
            {
                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, null as DateTime?)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create(),

                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, null as DateTime?)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create()
            };

            List<UserWordsResponse> userWords_response_list_expected = userWords.Select(
                temp => temp.ToUserWordsResponse(_userWordsService)).OrderByDescending(temp => temp.Priority).ToList();

            _userWordsRepositoryMock.Setup(temp => temp.GetAllUserWords()).Returns(userWords);

            List<UserWordsResponse> allUserWords = _userWordsService.GetAllUserWords();

            //Act
            List<UserWordsResponse> userWords_sorted = _userWordsService.GetUserWordsSortedByPriority(allUserWords);

            //Assert
            for (int i = 0; i < userWords_response_list_expected.Count; i++)
            {
                Assert.Equal(userWords_response_list_expected[i], userWords_sorted[i]);
                Assert.Equal(expectedPriority, userWords_sorted[i].Priority);
            }
        }

        /*[Fact]
        public void GetUserWordsSortedByPriority_UpdatedPriority()
        {
            //Arrange
            DateTime dateNow = DateTime.Now.Date;
            DateTime dateWeekAgo = DateTime.Now.Date.AddDays(-7);
            DateTime dateMonthAgo = DateTime.Now.Date.AddMonths(-1);

            int correctEnteredCountForNow = 1, incorrectEnteredCountForNow = 3;
            int correctEnteredCountForWeekAgo = 14, incorrectEnteredCountForWeekAgo = 4;
            int correctEnteredCountForMonthAgo = 16, incorrectEnteredCountForMonthAgo = 120;

            List<UserWords> userWords = new List<UserWords>()
            {
                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, dateMonthAgo)
                .With(temp => temp.CorrectEnteredCount, correctEnteredCountForMonthAgo)
                .With(temp => temp.IncorrectEnteredCount, incorrectEnteredCountForMonthAgo)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create(),

                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, dateWeekAgo)
                .With(temp => temp.CorrectEnteredCount, correctEnteredCountForWeekAgo)
                .With(temp => temp.IncorrectEnteredCount, incorrectEnteredCountForWeekAgo)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create(),

                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, dateNow)
                .With(temp => temp.CorrectEnteredCount, correctEnteredCountForNow)
                .With(temp => temp.IncorrectEnteredCount, incorrectEnteredCountForNow)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create(),
            };

            List<UserWordsResponse> userWords_list_expected = userWords.Select(
                temp => temp.ToUserWordsResponse(_userWordsService)).ToList();


            _userWordsRepositoryMock.Setup(temp => temp.GetAllUserWords()).Returns(userWords);


            List<UserWordsResponse> allUserWords = _userWordsService.GetAllUserWords();

            //Act
            List<UserWordsResponse> userWords_sorted = _userWordsService.GetUserWordsSortedByPriority(allUserWords);

            //Assert
            for (int i = 0; i < userWords_list_expected.Count; i++)
            {
                Assert.Equal(userWords_list_expected[i], userWords_sorted[i]);
            }
        }*/

        #endregion

        #region GetAllUserWords

        [Fact]
        public void GetAllUsersWords_EmptyList()
        {
            //Arrange
            List<UserWords> userWords = new List<UserWords>();

            _userWordsRepositoryMock.Setup(temp => temp.GetAllUserWords()).Returns(userWords);

            //Act
            List<UserWordsResponse> actual_userWords_response_list = _userWordsService.GetAllUserWords();

            //Assert
            Assert.Empty(actual_userWords_response_list);
        }

        [Fact]
        public void GetAllUserWords_AddFewUserWords()
        {
            //Arrange
            List<UserWords> userWords = new List<UserWords>()
            {
                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-2))
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create(),

                _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-2))
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create()
            };

            List<UserWordsResponse> userWords_response_list_expected = userWords.Select(
                temp => temp.ToUserWordsResponse(_userWordsService)).ToList();

            _userWordsRepositoryMock.Setup(temp => temp.GetAllUserWords()).Returns(userWords);

            //Act
            List<UserWordsResponse> userWords_from_get = _userWordsService.GetAllUserWords();

            //Assert
            Assert.Equal(userWords_response_list_expected, userWords_from_get);
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

            UserWords userWord = _fixture.Build<UserWords>()
                .With(temp => temp.CorrectEnteredCount, 0)
                .With(temp => temp.IncorrectEnteredCount, 0)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users)
                .Create();

            UserWordsResponse userWord_response_from_add = userWord.ToUserWordsResponse(_userWordsService);

            UserWordsUpdateRequest userWord_update_request = userWord_response_from_add.ToUserWordsUpdateRequest();

            _userWordsRepositoryMock.Setup(temp => temp.UpdateUserWord(It.IsAny<UserWords>())).Returns(userWord);
            _userWordsRepositoryMock.Setup(temp => temp.GetUserWordById(It.IsAny<Guid>())).Returns(userWord);

            userWord_update_request.IsIncreaseCorrectEnteredCount = true;

            //Act
            UserWordsResponse userWords_response_from_update = _userWordsService.UpdateUserWord(userWord_update_request);
            userWord_response_from_add.CorrectEnteredCount++;

            //Assert
            Assert.Equal(expected, userWords_response_from_update.CorrectEnteredCount);
            Assert.Equal(expected1, userWords_response_from_update.IncorrectEnteredCount);
        }

        [Fact]
        public void UpdateUserWord_IncreaseCorrectEnteredCountThenIncorrect()
        {
            //Arrange
            int expected = 1;
            int expected1 = 1;

            UserWords userWord = _fixture.Build<UserWords>()
                .With(temp => temp.CorrectEnteredCount, 0)
                .With(temp => temp.IncorrectEnteredCount, 0)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users)
                .Create();

            UserWordsResponse userWord_response_from_add = userWord.ToUserWordsResponse(_userWordsService);

            UserWordsUpdateRequest userWord_update_request1 = userWord_response_from_add.ToUserWordsUpdateRequest();

            _userWordsRepositoryMock.Setup(temp => temp.UpdateUserWord(It.IsAny<UserWords>())).Returns(userWord);
            _userWordsRepositoryMock.Setup(temp => temp.GetUserWordById(It.IsAny<Guid>())).Returns(userWord);

            userWord_update_request1.IsIncreaseCorrectEnteredCount = true;

            UserWordsResponse userWords_response1_from_update = _userWordsService.UpdateUserWord(userWord_update_request1);
            userWord_response_from_add.CorrectEnteredCount++;

            UserWordsUpdateRequest userWord_update_request2 = userWords_response1_from_update.ToUserWordsUpdateRequest();

            userWord_update_request2.IsIncreaseIncorrectEnteredCount = true;

            //Act
            UserWordsResponse userWords_response2_from_update = _userWordsService.UpdateUserWord(userWord_update_request2);
            userWord_response_from_add.CorrectEnteredCount++;

            //Assert
            Assert.Equal(expected, userWords_response2_from_update.CorrectEnteredCount);
            Assert.Equal(expected1, userWords_response2_from_update.IncorrectEnteredCount);
        }

        [Fact]
        public void UpdateUserWord_UpdateLastTimeEntered()
        {
            //Arrange
            DateTime expectedDate = DateTime.Now;
            UserWords userWord = _fixture.Build<UserWords>()
                .With(temp => temp.LastTimeEntered, expectedDate)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create();

            UserWordsResponse userWord_response_expected = userWord.ToUserWordsResponse(_userWordsService);

            UserWordsUpdateRequest userWord_update_request = userWord_response_expected.ToUserWordsUpdateRequest();

            _userWordsRepositoryMock.Setup(temp => temp.UpdateUserWord(It.IsAny<UserWords>())).Returns(userWord);
            _userWordsRepositoryMock.Setup(temp => temp.GetUserWordById(It.IsAny<Guid>())).Returns(userWord);

            //Act
            UserWordsResponse userWords_response_from_update = _userWordsService.UpdateUserWord(userWord_update_request);

            //Assert
            Assert.Equal(expectedDate.Date, userWords_response_from_update.LastTimeEntered!.Value.Date);
            Assert.Equal(expectedDate.Hour, userWords_response_from_update.LastTimeEntered!.Value.Hour);
            Assert.Equal(expectedDate.Minute, userWords_response_from_update.LastTimeEntered!.Value.Minute);
        }

        #endregion

        #region DeleteDefaultWord

        [Fact]
        public void DeleteUserWord_ValidId()
        {
            //Arrange
            UserWords userWord = _fixture.Build<UserWords>()
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users)
                .Create();

            _userWordsRepositoryMock.Setup(temp => temp.DeleteUserWord(It.IsAny<Guid>())).Returns(true);
            _userWordsRepositoryMock.Setup(temp => temp.GetUserWordById(It.IsAny<Guid>())).Returns(userWord);

            //Act
            bool isDeleted = _userWordsService.DeleteUserWord(userWord.UserWordId);

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