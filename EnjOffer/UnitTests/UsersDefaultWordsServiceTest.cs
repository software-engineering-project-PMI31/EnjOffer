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
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;
using EnjOffer.Core.Domain.IdentityEntities;

namespace UnitTests
{
    public class UsersDefaultWordsServiceTest
    {
        private readonly Mock<IUsersDefaultWordsRepository> _usersDefaultWordsRepositoryMock;
        private readonly IUsersDefaultWordsRepository _usersDefaultWordsRepository;
        private readonly IUsersDefaultWordsService _usersDefaultWordsService;

        private readonly Mock<IDefaultWordsRepository> _defaultWordsRepositoryMock;
        private readonly IDefaultWordsRepository _defaultWordsRepository;
        private readonly IDefaultWordsService _defaultWordsService;
        private readonly IFixture _fixture;

        public UsersDefaultWordsServiceTest()
        {
            _fixture = new Fixture();

            _usersDefaultWordsRepositoryMock = new Mock<IUsersDefaultWordsRepository>();
            _usersDefaultWordsRepository = _usersDefaultWordsRepositoryMock.Object;

            _defaultWordsRepositoryMock = new Mock<IDefaultWordsRepository>();
            _defaultWordsRepository = _defaultWordsRepositoryMock.Object;

            _usersDefaultWordsService = new UsersDefaultWordsService(_usersDefaultWordsRepository, _defaultWordsRepository);

        }

        #region AddUsersDefaultWord

        [Fact]
        public void AddUsersDefaultWord_NullUsersDefaultWord()
        {
            //Arrange
            UsersDefaultWordsAddRequest? usersDefaultWordAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _usersDefaultWordsService.AddUserDefaultWord(usersDefaultWordAddRequest);
            });
        }

        [Fact]
        public void AddUserWord_ProperDetails()
        {
            //Arrange
            UsersDefaultWordsAddRequest? usersDefaultWord_add_request = _fixture.Create<UsersDefaultWordsAddRequest>();
            UsersDefaultWords usersDefaultWord = usersDefaultWord_add_request.ToUsersDefaultWords();

            UsersDefaultWordsResponse usersDefaultWord_response_expected = 
                usersDefaultWord.ToUserDefaultWordsResponse(_usersDefaultWordsService);

            _usersDefaultWordsRepositoryMock.Setup(temp => 
                temp.AddUserDefaultWord(It.IsAny<UsersDefaultWords>())).Returns(usersDefaultWord);


            //Act
            UsersDefaultWordsResponse usersDefaultWord_response_from_add =
                _usersDefaultWordsService.AddUserDefaultWord(usersDefaultWord_add_request);

            usersDefaultWord_response_expected.DefaultWordId = usersDefaultWord_response_from_add.DefaultWordId;
            usersDefaultWord_response_expected.UserId = usersDefaultWord_response_from_add.UserId;

            //Assert
            Assert.True(usersDefaultWord_response_from_add.DefaultWordId != Guid.Empty);
            Assert.True(usersDefaultWord_response_from_add.UserId != Guid.Empty);
            Assert.Equal(usersDefaultWord_response_expected, usersDefaultWord_response_from_add);
        }

        [Fact]
        public void AddUsersDefaultWord_DuplicateValues()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            UsersDefaultWordsAddRequest? request1 = new UsersDefaultWordsAddRequest()
            {
                DefaultWordId = guid,
                UserId = guid

            };
            UsersDefaultWordsAddRequest? request2 = new UsersDefaultWordsAddRequest()
            {
                DefaultWordId = guid,
                UserId = guid
            };

            UsersDefaultWords first_usersDefaultWord = request1.ToUsersDefaultWords();

            _usersDefaultWordsRepositoryMock.SetupSequence(temp => temp.AddUserDefaultWord(It.IsAny<UsersDefaultWords>()))
            .Throws(new ArgumentException("The record with such default word and user already exist"))
            .Returns(first_usersDefaultWord);

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _usersDefaultWordsService.AddUserDefaultWord(request2);
            });
        }

        #endregion

        #region GetPriority

        //TO-DO: Consider making the test as integrational
        [Fact]
        public void GetPriority_NullDate()
        {
            //Arrange
            UsersDefaultWordsAddRequest usersDefaultWordRequest = _fixture.Create<UsersDefaultWordsAddRequest>();
            UsersDefaultWords usersDefaultWord = usersDefaultWordRequest.ToUsersDefaultWords();

            _usersDefaultWordsRepositoryMock.Setup(temp => temp.AddUserDefaultWord(It.IsAny<UsersDefaultWords>())).Returns(usersDefaultWord);

            //Act
            UsersDefaultWordsResponse usersDefaultWord_from_add = _usersDefaultWordsService.AddUserDefaultWord(usersDefaultWordRequest);

            //Assert
            Assert.Equal(1, usersDefaultWord_from_add.Priority);
        }

        #endregion

        #region GetUsersDefaultWordById

        [Fact]
        public void GetUsersDefaultWordById_NullId()
        {
            //Arrange
            Guid? userId = null;
            Guid? defaultWordId = null;

            //Act
            UsersDefaultWordsResponse? usersDefaultWord_response_from_get = _usersDefaultWordsService.GetUserDefaultWordById(defaultWordId, userId);

            //Assert
            Assert.Null(usersDefaultWord_response_from_get);
        }

        [Fact]
        public void GetUsersDefaultWordById_ProperId()
        {
            //Arrange
            UsersDefaultWords usersDefaultWord = _fixture.Build<UsersDefaultWords>()
                .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-3))
                .With(temp => temp.DefaultWordId, Guid.Empty)
                .With(temp => temp.DefaultWord, null as DefaultWords)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as ApplicationUser)
                .Create();

            UsersDefaultWordsResponse usersDefaultWord_response_expected = usersDefaultWord.ToUserDefaultWordsResponse(_usersDefaultWordsService);

            _usersDefaultWordsRepositoryMock.Setup(temp => temp.GetUserDefaultWordById(
                It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(usersDefaultWord);

            //Act
            UsersDefaultWordsResponse? usersDefaultWord_from_get = _usersDefaultWordsService.GetUserDefaultWordById(usersDefaultWord.DefaultWordId, usersDefaultWord.UserId);

            //Assert
            Assert.Equal(usersDefaultWord_response_expected, usersDefaultWord_from_get);
        }

        #endregion

        #region GetUsersDefaultWordsByDate

        [Fact]
        public void GetUsersDefaultWordsByDate_NullDate()
        {
            //Arrange
            DateTime? date = null;

            List<UsersDefaultWords> usersDefaultWords = new List<UsersDefaultWords>()
            {
                _fixture.Build<UsersDefaultWords>()
                    .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-3))
                    .With(temp => temp.DefaultWordId, Guid.Empty)
                    .With(temp => temp.DefaultWord, null as DefaultWords)
                    .With(temp => temp.UserId, Guid.Empty)
                    .With(temp => temp.User, null as ApplicationUser)
                    .Create(),

                _fixture.Build<UsersDefaultWords>()
                    .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-3))
                    .With(temp => temp.DefaultWordId, Guid.Empty)
                    .With(temp => temp.DefaultWord, null as DefaultWords)
                    .With(temp => temp.UserId, Guid.Empty)
                    .With(temp => temp.User, null as ApplicationUser)
                    .Create()
        };

            _usersDefaultWordsRepositoryMock.Setup(temp =>
                temp.GetUserDefaultWordsByDate(It.IsAny<DateTime?>())).Returns(usersDefaultWords);

            List<UsersDefaultWordsResponse> usersDefaultWords_response_list_expected = usersDefaultWords.Select(
                temp => temp.ToUserDefaultWordsResponse(_usersDefaultWordsService)).ToList();

            //Act
            List<UsersDefaultWordsResponse> usersDefaultWords_from_get = _usersDefaultWordsService.GetUserDefaultWordsByDate(date);

            //Assert
            Assert.Equal(usersDefaultWords_response_list_expected, usersDefaultWords_from_get);
        }

        [Fact]
        public void GetUserWordsByDate_ProperDate()
        {
            //Arrange
            DateTime dateNow = DateTime.Now.Date;

            List<UsersDefaultWords> usersDefaultWords = new List<UsersDefaultWords>()
            {
                _fixture.Build<UsersDefaultWords>()
                    .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-3))
                    .With(temp => temp.DefaultWordId, Guid.Empty)
                    .With(temp => temp.DefaultWord, null as DefaultWords)
                    .With(temp => temp.UserId, Guid.Empty)
                    .With(temp => temp.User, null as ApplicationUser)
                    .Create(),

                _fixture.Build<UsersDefaultWords>()
                    .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-3))
                    .With(temp => temp.DefaultWordId, Guid.Empty)
                    .With(temp => temp.DefaultWord, null as DefaultWords)
                    .With(temp => temp.UserId, Guid.Empty)
                    .With(temp => temp.User, null as ApplicationUser)
                    .Create()
            };

            _usersDefaultWordsRepositoryMock.Setup(temp => temp.GetUserDefaultWordsByDate(It.IsAny<DateTime?>()))
                .Returns(usersDefaultWords);

            List<UsersDefaultWordsResponse> usersDefaultWords_response_list_expected = usersDefaultWords.Select(
                temp => temp.ToUserDefaultWordsResponse(_usersDefaultWordsService)).ToList();

            //Act
            List<UsersDefaultWordsResponse> usersDefaultWords_from_get = _usersDefaultWordsService.GetUserDefaultWordsByDate(dateNow);

            //Assert
            Assert.Equal(usersDefaultWords_response_list_expected, usersDefaultWords_from_get);
        }

        #endregion

        #region GetUsersDefaultWordsSortedByPriority
        [Fact]
        public void GetUsersDefaultWordsSortedByPriority_DefaultPriority()
        {
            //Arrange
            int expectedPriority = 1;
            List<UsersDefaultWords> usersDefaultWords = new List<UsersDefaultWords>()
            {
                _fixture.Build<UsersDefaultWords>()
                .With(temp => temp.LastTimeEntered, null as DateTime?)
                .With(temp => temp.DefaultWordId, Guid.Empty)
                .With(temp => temp.DefaultWord, null as DefaultWords)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as ApplicationUser).Create(),

                _fixture.Build<UsersDefaultWords>()
                .With(temp => temp.LastTimeEntered, null as DateTime?)
                .With(temp => temp.DefaultWordId, Guid.Empty)
                .With(temp => temp.DefaultWord, null as DefaultWords)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as ApplicationUser).Create()
            };

            List<UsersDefaultWordsResponse> usersDefaultWords_response_list_expected = usersDefaultWords.Select(
                temp => temp.ToUserDefaultWordsResponse(_usersDefaultWordsService)).OrderByDescending(temp => temp.Priority).ToList();

            _usersDefaultWordsRepositoryMock.Setup(temp => temp.GetAllUserDefaultWords()).Returns(usersDefaultWords);

            List<UsersDefaultWordsResponse> allUsersDefaultWords = _usersDefaultWordsService.GetAllUserDefaultWords();

            //Act
            List<UsersDefaultWordsResponse> usersDefaultWords_sorted =
                _usersDefaultWordsService.GetUserDefaultWordsSortedByPriority(allUsersDefaultWords);

            //Assert
            for (int i = 0; i < usersDefaultWords_response_list_expected.Count; i++)
            {
                Assert.Equal(usersDefaultWords_response_list_expected[i], usersDefaultWords_sorted[i]);
                Assert.Equal(expectedPriority, usersDefaultWords_sorted[i].Priority);
            }
        }

        /*[Fact]
        public void GetUsersDefaultWordsSortedByPriority_UpdatedPriority()
        {
            //Arrange
            DateTime dateNow = DateTime.Now.Date;
            DateTime dateWeekAgo = DateTime.Now.Date.AddDays(-7);
            DateTime dateMonthAgo = DateTime.Now.Date.AddMonths(-1);

            int correctEnteredCountForNow = 1, incorrectEnteredCountForNow = 3;
            int correctEnteredCountForWeekAgo = 14, incorrectEnteredCountForWeekAgo = 4;
            int correctEnteredCountForMonthAgo = 16, incorrectEnteredCountForMonthAgo = 120;

            List<UsersDefaultWords> usersDefaultWords = new List<UsersDefaultWords>()
            {
                _fixture.Build<UsersDefaultWords>()
                .With(temp => temp.LastTimeEntered, dateMonthAgo)
                .With(temp => temp.CorrectEnteredCount, correctEnteredCountForMonthAgo)
                .With(temp => temp.IncorrectEnteredCount, incorrectEnteredCountForMonthAgo)
                .With(temp => temp.DefaultWordId, Guid.Empty)
                .With(temp => temp.DefaultWord, null as DefaultWords)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as ApplicationUser).Create(),

                _fixture.Build<UsersDefaultWords>()
                .With(temp => temp.LastTimeEntered, dateWeekAgo)
                .With(temp => temp.CorrectEnteredCount, correctEnteredCountForWeekAgo)
                .With(temp => temp.IncorrectEnteredCount, incorrectEnteredCountForWeekAgo)
                .With(temp => temp.DefaultWordId, Guid.Empty)
                .With(temp => temp.DefaultWord, null as DefaultWords)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as ApplicationUser).Create(),

                _fixture.Build<UsersDefaultWords>()
                .With(temp => temp.LastTimeEntered, dateNow)
                .With(temp => temp.CorrectEnteredCount, correctEnteredCountForNow)
                .With(temp => temp.IncorrectEnteredCount, incorrectEnteredCountForNow)
                .With(temp => temp.DefaultWordId, Guid.Empty)
                .With(temp => temp.DefaultWord, null as DefaultWords)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as ApplicationUser).Create(),
            };

            List<UsersDefaultWordsResponse> usersDefaultWords_list_expected = usersDefaultWords.Select(
                temp => temp.ToUserDefaultWordsResponse(_usersDefaultWordsService)).ToList();


            _usersDefaultWordsRepositoryMock.Setup(temp => temp.GetAllUserDefaultWords()).Returns(usersDefaultWords);


            List<UsersDefaultWordsResponse> allUsersDefaultWords = _usersDefaultWordsService.GetAllUserDefaultWords();

            //Act
            List<UsersDefaultWordsResponse> usersDefaultWords_sorted =
                _usersDefaultWordsService.GetUserDefaultWordsSortedByPriority(allUsersDefaultWords);

            //Assert
            for (int i = 0; i < usersDefaultWords_list_expected.Count; i++)
            {
                Assert.Equal(usersDefaultWords_list_expected[i], usersDefaultWords_sorted[i]);
            }
        }*/

        #endregion

        #region GetAllUsersDefaultWords

        [Fact]
        public void GetAllUsersDefaultWords_EmptyList()
        {
            //Arrange
            List<UsersDefaultWords> usersDefaultWords = new List<UsersDefaultWords>();

            _usersDefaultWordsRepositoryMock.Setup(temp => temp.GetAllUserDefaultWords()).Returns(usersDefaultWords);

            //Act
            List<UsersDefaultWordsResponse> actual_usersDefaultWords_response_list =
                _usersDefaultWordsService.GetAllUserDefaultWords();

            //Assert
            Assert.Empty(actual_usersDefaultWords_response_list);
        }

        [Fact]
        public void GetAllUsersDefaultWords_AddFewUsersDefaultWords()
        {
            //Arrange
            List<UsersDefaultWords> usersDefaultWords = new List<UsersDefaultWords>()
            {
                _fixture.Build<UsersDefaultWords>()
                    .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-2))
                    .With(temp => temp.DefaultWordId, Guid.Empty)
                    .With(temp => temp.DefaultWord, null as DefaultWords)
                    .With(temp => temp.UserId, Guid.Empty)
                    .With(temp => temp.User, null as ApplicationUser)
                    .Create(),

                _fixture.Build<UsersDefaultWords>()
                    .With(temp => temp.LastTimeEntered, DateTime.Now.AddDays(-2))
                    .With(temp => temp.DefaultWordId, Guid.Empty)
                    .With(temp => temp.DefaultWord, null as DefaultWords)
                    .With(temp => temp.UserId, Guid.Empty)
                    .With(temp => temp.User, null as ApplicationUser)
                    .Create()
            };

            List<UsersDefaultWordsResponse> usersDefaultWords_response_list_expected = usersDefaultWords.Select(
                temp => temp.ToUserDefaultWordsResponse(_usersDefaultWordsService)).ToList();

            _usersDefaultWordsRepositoryMock.Setup(temp => temp.GetAllUserDefaultWords()).Returns(usersDefaultWords);

            //Act
            List<UsersDefaultWordsResponse> usersDefaultWords_from_get = _usersDefaultWordsService.GetAllUserDefaultWords();

            //Assert
            Assert.Equal(usersDefaultWords_response_list_expected, usersDefaultWords_from_get);
        }

        #endregion

        #region UpdateUserWord

        [Fact]
        public void UpdateUsersDefaultWord_NullUsersDefaultWord()
        {
            //Arrange
            UsersDefaultWordsUpdateRequest? usersDefaultWord_update_request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _usersDefaultWordsService.UpdateUserDefaultWord(usersDefaultWord_update_request);
            });
        }

        [Fact]
        public void UpdateUsersDefaultWord_InvalidUserId()
        {
            //Arrange
            UsersDefaultWordsUpdateRequest? usersDefaultWord_update_request = new UsersDefaultWordsUpdateRequest()
            {
                UserId = Guid.NewGuid()
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _usersDefaultWordsService.UpdateUserDefaultWord(usersDefaultWord_update_request);
            });
        }

        [Fact]
        public void UpdateUsersDefaultWord_InvalidDefaultWordId()
        {
            //Arrange
            UsersDefaultWordsUpdateRequest? usersDefaultWord_update_request = new UsersDefaultWordsUpdateRequest()
            {
                DefaultWordId = Guid.NewGuid()
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _usersDefaultWordsService.UpdateUserDefaultWord(usersDefaultWord_update_request);
            });
        }

        [Fact]
        public void UpdateUsersDefaultWord_IncreaseBothCorrectAndIncorrect()
        {
            //Arrange
            UsersDefaultWordsUpdateRequest usersDefaultWord_update_request = new UsersDefaultWordsUpdateRequest()
            {
                IsIncreaseCorrectEnteredCount = true,
                IsIncreaseIncorrectEnteredCount = true
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _usersDefaultWordsService.UpdateUserDefaultWord(usersDefaultWord_update_request);
            });
        }

        [Fact]
        public void UpdateUsersDefaultWord_IncreaseCorrectEnteredCount()
        {
            //Arrange
            int expected = 1;
            int expected1 = 0;

            UsersDefaultWords usersDefaultWord = _fixture.Build<UsersDefaultWords>()
                .With(temp => temp.CorrectEnteredCount, 0)
                .With(temp => temp.IncorrectEnteredCount, 0)
                .With(temp => temp.DefaultWordId, Guid.Empty)
                .With(temp => temp.DefaultWord, null as DefaultWords)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as ApplicationUser)
                .Create();

            UsersDefaultWordsResponse usersDefaultWord_response_from_add =
                usersDefaultWord.ToUserDefaultWordsResponse(_usersDefaultWordsService);

            UsersDefaultWordsUpdateRequest usersDefaultWord_update_request =
                usersDefaultWord_response_from_add.ToUsersDefaultWordsWordsUpdateRequest();

            _usersDefaultWordsRepositoryMock.Setup(temp => temp.UpdateUserDefaultWord(It.IsAny<UsersDefaultWords>()))
                .Returns(usersDefaultWord);
            _usersDefaultWordsRepositoryMock.Setup(temp => temp.GetUserDefaultWordById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(usersDefaultWord);

            usersDefaultWord_update_request.IsIncreaseCorrectEnteredCount = true;

            //Act
            UsersDefaultWordsResponse usersDefaultWords_response_from_update =
                _usersDefaultWordsService.UpdateUserDefaultWord(usersDefaultWord_update_request);
            usersDefaultWord_response_from_add.CorrectEnteredCount++;

            //Assert
            Assert.Equal(expected, usersDefaultWords_response_from_update.CorrectEnteredCount);
            Assert.Equal(expected1, usersDefaultWords_response_from_update.IncorrectEnteredCount);
        }

        [Fact]
        public void UpdateUsersDefaultWord_IncreaseCorrectEnteredCountThenIncorrect()
        {
            //Arrange
            int expected = 1;

            UsersDefaultWords usersDefaultWord = _fixture.Build<UsersDefaultWords>()
                .With(temp => temp.CorrectEnteredCount, 0)
                .With(temp => temp.IncorrectEnteredCount, 0)
                .With(temp => temp.DefaultWordId, Guid.Empty)
                .With(temp => temp.DefaultWord, null as DefaultWords)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as ApplicationUser)
                .Create();

            UsersDefaultWordsResponse usersDefaultWord_response_from_add =
                usersDefaultWord.ToUserDefaultWordsResponse(_usersDefaultWordsService);

            UsersDefaultWordsUpdateRequest usersDefaultWord_update_request1 =
                usersDefaultWord_response_from_add.ToUsersDefaultWordsWordsUpdateRequest();

            _usersDefaultWordsRepositoryMock.Setup(temp => temp.UpdateUserDefaultWord(It.IsAny<UsersDefaultWords>()))
                .Returns(usersDefaultWord);
            _usersDefaultWordsRepositoryMock.Setup(temp => temp.GetUserDefaultWordById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(usersDefaultWord);

            usersDefaultWord_update_request1.IsIncreaseCorrectEnteredCount = true;

            UsersDefaultWordsResponse usersDefaultWords_response1_from_update =
                _usersDefaultWordsService.UpdateUserDefaultWord(usersDefaultWord_update_request1);
            usersDefaultWord_response_from_add.CorrectEnteredCount++;

            UsersDefaultWordsUpdateRequest usersDefaultWord_update_request2 =
                usersDefaultWords_response1_from_update.ToUsersDefaultWordsWordsUpdateRequest();

            usersDefaultWord_update_request2.IsIncreaseIncorrectEnteredCount = true;

            //Act
            UsersDefaultWordsResponse usersDefaultWords_response2_from_update =
                _usersDefaultWordsService.UpdateUserDefaultWord(usersDefaultWord_update_request2);
            usersDefaultWord_response_from_add.CorrectEnteredCount++;

            //Assert
            Assert.Equal(expected, usersDefaultWords_response2_from_update.CorrectEnteredCount);
            Assert.Equal(expected, usersDefaultWords_response2_from_update.IncorrectEnteredCount);
        }

        [Fact]
        public void UpdateUsersDefaultWord_UpdateLastTimeEntered()
        {
            //Arrange
            DateTime expectedDate = DateTime.Now;
            UsersDefaultWords usersDefaultWord = _fixture.Build<UsersDefaultWords>()
                .With(temp => temp.LastTimeEntered, expectedDate)
                .With(temp => temp.DefaultWordId, Guid.Empty)
                .With(temp => temp.DefaultWord, null as DefaultWords)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as ApplicationUser).Create();

            UsersDefaultWordsResponse usersDefaultWord_response_expected =
                usersDefaultWord.ToUserDefaultWordsResponse(_usersDefaultWordsService);

            UsersDefaultWordsUpdateRequest usersDefaultWord_update_request =
                usersDefaultWord_response_expected.ToUsersDefaultWordsWordsUpdateRequest();

            _usersDefaultWordsRepositoryMock.Setup(temp => temp.UpdateUserDefaultWord(It.IsAny<UsersDefaultWords>()))
                .Returns(usersDefaultWord);
            _usersDefaultWordsRepositoryMock.Setup(temp => temp.GetUserDefaultWordById(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(usersDefaultWord);

            //Act
            UsersDefaultWordsResponse usersDefaultWords_response_from_update = 
                _usersDefaultWordsService.UpdateUserDefaultWord(usersDefaultWord_update_request);

            //Assert
            Assert.Equal(expectedDate.Date, usersDefaultWords_response_from_update.LastTimeEntered!.Value.Date);
            Assert.Equal(expectedDate.Hour, usersDefaultWords_response_from_update.LastTimeEntered!.Value.Hour);
            Assert.Equal(expectedDate.Minute, usersDefaultWords_response_from_update.LastTimeEntered!.Value.Minute);
        }

        #endregion
    }
}
