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

namespace UnitTests
{
    public class UserStatisticsServiceTest
    {
        private readonly Mock<IUserStatisticsRepository> _userStatisticsRepositoryMock;
        private readonly IUserStatisticsRepository _userStatisticsRepository;
        private readonly IUserStatisticsService _userStatisticsService;
        private readonly IFixture _fixture;

        public UserStatisticsServiceTest()
        {
            _fixture = new Fixture();

            _userStatisticsRepositoryMock = new Mock<IUserStatisticsRepository>();
            _userStatisticsRepository = _userStatisticsRepositoryMock.Object;

            _userStatisticsService = new UserStatisticsService(_userStatisticsRepository);
        }

        #region AddUserStatistics

        [Fact]
        public void AddUserStatistics_NullUserStatistics()
        {
            //Arrange
            UserStatisticsAddRequest? userStatisticsAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _userStatisticsService.AddUserStatistics(userStatisticsAddRequest);
            });
        }

        [Fact]
        public void AddUsertStatistics_DuplicateStatistics()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            UserStatisticsAddRequest? request1 = new UserStatisticsAddRequest()
            {
                AnswerDate = DateTime.Now.Date,
                UserId = guid
            };
            UserStatisticsAddRequest? request2 = new UserStatisticsAddRequest()
            {
                AnswerDate = DateTime.Now.Date,
                UserId = guid
            };

            UserStatistics fisrt_userStatistics = request1.ToUserStatistics();

            _userStatisticsRepositoryMock.SetupSequence(temp => temp.AddUserStatisticRecord(It.IsAny<UserStatistics>()))
                .Throws(new ArgumentException("The defaultStatistics with such date already exist"))
                .Returns(fisrt_userStatistics);

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _userStatisticsService.AddUserStatistics(request2);
            });
        }

        /*[Fact]
        public void AddUserStatistics_ProperDetails()
        {
            //Arrange
            UserStatisticsAddRequest? userStatistics_add_request =
                _fixture.Build<UserStatisticsAddRequest>()
                .With(temp => temp.AnswerDate, DateTime.Now.Date)
                .Create();

            UserStatistics userStatistics = userStatistics_add_request.ToUserStatistics();

            UserStatisticsResponse userStatistics_response_expected =
                userStatistics.ToUserStatisticsResponse();

            _userStatisticsRepositoryMock.Setup(temp => temp.AddUserStatisticRecord(It.IsAny<UserStatistics>()))
                .Returns(userStatistics);


            //Act
            UserStatisticsResponse userStatistics_response_from_add =
                _userStatisticsService.AddUserStatistics(userStatistics_add_request);

            userStatistics_response_expected.UserStatisticsId = userStatistics_response_from_add.UserStatisticsId;

            //Assert
            Assert.True(userStatistics_response_from_add.UserStatisticsId != Guid.Empty);
            Assert.Equal(userStatistics_response_expected, userStatistics_response_from_add);
        }*/

        #endregion

        #region GetUserStatisticsById

        [Fact]
        public void GetUserStatisticsById_NullId()
        {
            //Arrange
            Guid? userStatisticsId = null;

            //Act
            UserStatisticsResponse? userStatistics_response_from_get =
                _userStatisticsService.GetUserStatisticsById(userStatisticsId);

            //Assert
            Assert.Null(userStatistics_response_from_get);
        }

        [Fact]
        public void GetUserStatisticsById_ProperId()
        {
            //Arrange
            UserStatistics userStatistics = _fixture.Build<UserStatistics>()
                .With(temp => temp.AnswerDate, DateTime.Now.Date)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users)
                .Create();

            UserStatisticsResponse userStatistics_response_expected = userStatistics.ToUserStatisticsResponse();

            _userStatisticsRepositoryMock.Setup(temp => temp.GetStatisticsById(It.IsAny<Guid>())).Returns(userStatistics);

            //Act
            UserStatisticsResponse? userStatistics_from_get = 
                _userStatisticsService.GetUserStatisticsById(userStatistics.UserStatisticsId);

            //Assert
            Assert.Equal(userStatistics_response_expected, userStatistics_from_get);
        }

        #endregion

        #region GetUserStatisticsByDate

        [Fact]
        public void GetUserStatisticsByDate_NullDate()
        {
            //Arrange
            DateTime? date = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _userStatisticsService.GetUserStatisticsByDate(date);
            });
        }

        [Fact]
        public void GetUserStatisticsByDate_ProperDate()
        {
            //Arrange
            DateTime dateNow = DateTime.Now.Date;

            UserStatistics userStatistics = 
                _fixture.Build<UserStatistics>()
                    .With(temp => temp.AnswerDate, dateNow)
                    .With(temp => temp.UserId, Guid.Empty)
                    .With(temp => temp.User, null as Users)
                    .Create();

            _userStatisticsRepositoryMock.Setup(temp => temp.GetUserStatisticsByDate(It.IsAny<DateTime?>())).Returns(userStatistics);

            UserStatisticsResponse userStatistics_response_list_expected = 
                userStatistics.ToUserStatisticsResponse();

            //Act
            UserStatisticsResponse? userStatistics_from_get = _userStatisticsService.GetUserStatisticsByDate(dateNow);

            //Assert
            Assert.Equal(userStatistics_response_list_expected, userStatistics_from_get);
        }

        #endregion

        #region GetAllUserStatistics

        [Fact]
        public void GetAllUsersStatistics_EmptyList()
        {
            //Arrange
            List<UserStatistics> userStatistics = new List<UserStatistics>();

            _userStatisticsRepositoryMock.Setup(temp => temp.GetAllUserStatistics()).Returns(userStatistics);

            //Act
            List<UserStatisticsResponse> actual_userStatistics_response_list = _userStatisticsService.GetAllUserStatistics();

            //Assert
            Assert.Empty(actual_userStatistics_response_list);
        }

        [Fact]
        public void GetAllUserStatistics_AddFewUserStatistics()
        {
            //Arrange
            List<UserStatistics> userStatistics = new List<UserStatistics>()
            {
                _fixture.Build<UserStatistics>()
                .With(temp => temp.AnswerDate, DateTime.Now.AddDays(-2).Date)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create(),

                _fixture.Build<UserStatistics>()
                .With(temp => temp.AnswerDate, DateTime.Now.Date)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users).Create()
            };

            List<UserStatisticsResponse> userStatistics_response_list_expected = userStatistics.Select(
                temp => temp.ToUserStatisticsResponse()).ToList();

            _userStatisticsRepositoryMock.Setup(temp => temp.GetAllUserStatistics()).Returns(userStatistics);

            //Act
            List<UserStatisticsResponse> userStatistics_from_get = _userStatisticsService.GetAllUserStatistics();

            //Assert
            Assert.Equal(userStatistics_response_list_expected, userStatistics_from_get);
        }

        #endregion

        #region UpdateUserStatistics

        [Fact]
        public void UpdateUserStatistics_NullUserStatistics()
        {
            //Arrange
            UserStatisticsUpdateRequest? userStatistics_update_request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _userStatisticsService.UpdateUserStatistics(userStatistics_update_request);
            });
        }

        [Fact]
        public void UpdateUserStatistics_InvalidUserStatisticsId()
        {
            //Arrange
            UserStatisticsUpdateRequest? userStatistics_update_request = new UserStatisticsUpdateRequest()
            {
                UserStatisticsId = Guid.NewGuid()
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _userStatisticsService.UpdateUserStatistics(userStatistics_update_request);
            });
        }

        [Fact]
        public void UpdateUserStatistics_IncreaseCorrectEnteredCount()
        {
            //Arrange
            int expected = 2;
            int expected1 = 1;

            UserStatistics userStatistics = _fixture.Build<UserStatistics>()
                .With(temp => temp.CorrectAnswersCount, 2)
                .With(temp => temp.IncorrectAnswersCount, 1)
                .With(temp => temp.UserId, Guid.Empty)
                .With(temp => temp.User, null as Users)
                .Create();

            UserStatisticsResponse userStatistics_response_from_add = userStatistics.ToUserStatisticsResponse();

            UserStatisticsUpdateRequest userStatistics_update_request = userStatistics_response_from_add.ToUserStatisticsUpdateRequest();

            _userStatisticsRepositoryMock.Setup(temp => temp.UpdateUserStatistics(It.IsAny<UserStatistics>())).Returns(userStatistics);
            _userStatisticsRepositoryMock.Setup(temp => temp.GetStatisticsById(It.IsAny<Guid>())).Returns(userStatistics);

            userStatistics_update_request.CorrectAnswersCount = 2;
            userStatistics_update_request.IncorrectAnswersCount = 1;

            //Act
            UserStatisticsResponse userStatistics_response_from_update = _userStatisticsService.UpdateUserStatistics(userStatistics_update_request);

            //Assert
            Assert.Equal(expected, userStatistics_response_from_update.CorrectAnswersCount);
            Assert.Equal(expected1, userStatistics_response_from_update.IncorrectAnswersCount);
        }

        #endregion
    }
}
