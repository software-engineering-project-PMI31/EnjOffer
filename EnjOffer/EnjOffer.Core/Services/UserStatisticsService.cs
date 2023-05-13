using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.DTO;
using EnjOffer.Core.Helpers;
using EnjOffer.Core.ServiceContracts;

namespace EnjOffer.Core.Services
{
    public class UserStatisticsService : IUserStatisticsService
    {
        private readonly IUserStatisticsRepository _userStatisticsRepository;

        public UserStatisticsService(IUserStatisticsRepository userStatisticsRepository)
        {
            _userStatisticsRepository = userStatisticsRepository;
        }

        public UserStatisticsResponse AddUserStatistics(UserStatisticsAddRequest? userStatisticsAddRequest)
        {
            if (userStatisticsAddRequest is null)
            {
                throw new ArgumentNullException(nameof(userStatisticsAddRequest));
            }

            ValidationHelper.ModelValidation(userStatisticsAddRequest);

            if (userStatisticsAddRequest.AnswerDate is not null &&
                _userStatisticsRepository.GetUserStatisticsByDate(userStatisticsAddRequest.AnswerDate.Value.Date) is not null &&
                _userStatisticsRepository.GetStatisticsByDateAndUserId(userStatisticsAddRequest.AnswerDate.Value.Date, userStatisticsAddRequest.UserId) is not null)
            {
                /*throw new ArgumentException("This statistic record with such date already exist",
                    nameof(userStatisticsAddRequest));*/
                UserStatisticsUpdateRequest userStatisticsUpdateRequest = new UserStatisticsUpdateRequest()
                {
                    UserStatisticsId = _userStatisticsRepository
                        .GetStatisticsByDateAndUserId(userStatisticsAddRequest.AnswerDate.Value.Date, userStatisticsAddRequest.UserId)!.UserStatisticsId,
                    AnswerDate = userStatisticsAddRequest.AnswerDate,
                    IsIncreaseCorrectEnteredAnswers = userStatisticsAddRequest.IsIncreaseCorrectEnteredAnswers,
                    IsIncreaseIncorrectEnteredAnswers = userStatisticsAddRequest.IsIncreaseIncorrectEnteredAnswers,
                    CorrectAnswersCount = userStatisticsAddRequest.CorrectAnswersCount,
                    IncorrectAnswersCount = userStatisticsAddRequest.IncorrectAnswersCount,
                    UserId = userStatisticsAddRequest.UserId
                };

                return UpdateUserStatistics(userStatisticsUpdateRequest);
            }

            //Convert userWordAddRequest to UserWords type
            UserStatistics? userStatistics = userStatisticsAddRequest.ToUserStatistics();

            //Generate UserWordId
            userStatistics.UserStatisticsId = Guid.NewGuid();

            //Generate Datetime and default correct and incorrect answers
            userStatistics.AnswerDate = userStatisticsAddRequest.AnswerDate ?? DateTime.Now.Date;
            userStatistics.CorrectAnswersCount = userStatisticsAddRequest.CorrectAnswersCount ?? 0;
            userStatistics.IncorrectAnswersCount = userStatisticsAddRequest.IncorrectAnswersCount ?? 0;

            _userStatisticsRepository.AddUserStatisticRecord(userStatistics);

            return userStatistics.ToUserStatisticsResponse();
        }

        public List<UserStatisticsResponse> GetAllUserStatistics(Guid userId)
        {
            return _userStatisticsRepository.GetAllUserStatistics().Where(x => x.UserId == userId)
                .Select(userStatistic => userStatistic.ToUserStatisticsResponse()).ToList();
        }

        public UserStatisticsResponse? GetUserStatisticsByDate(DateTime? dateTime)
        {
            if (dateTime is null)
            {
                throw new ArgumentNullException(nameof(dateTime));
            }

            UserStatistics? userStatistics_response_from_list = 
                _userStatisticsRepository.GetUserStatisticsByDate(dateTime);

            return userStatistics_response_from_list?.ToUserStatisticsResponse() ?? null;
        }

        public UserStatisticsResponse? GetUserStatisticsByDateAndUserId(DateTime? dateTime, Guid? userId)
        {
            if (dateTime is null)
            {
                throw new ArgumentNullException(nameof(dateTime));
            }

            if (userId is null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            UserStatistics? userStatistics_response_from_list =
                _userStatisticsRepository.GetStatisticsByDateAndUserId(dateTime, userId);

            return userStatistics_response_from_list?.ToUserStatisticsResponse() ?? null;
        }

        public UserStatisticsResponse? GetUserStatisticsById(Guid? userStatisticsId)
        {
            if (userStatisticsId is null)
            {
                return null;
            }

            UserStatistics? userStatistics_response_from_list = _userStatisticsRepository.GetStatisticsById(userStatisticsId);

            return userStatistics_response_from_list?.ToUserStatisticsResponse() ?? null;
        }

        /*public UserStatisticsResponse? GetUserStatisticsByIdAndUserId(Guid? userStatisticsId, Guid userId)
        {
            if (userStatisticsId is null)
            {
                return null;
            }

            UserStatistics? userStatistics_response_from_list = _userStatisticsRepository.GetStatisticsByIdAndUserId(userStatisticsId, userId);

            return userStatistics_response_from_list?.ToUserStatisticsResponse() ?? null;
        }*/

        public UserStatisticsResponse UpdateUserStatistics(UserStatisticsUpdateRequest? userStatisticsUpdateRequest)
        {
            if (userStatisticsUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(userStatisticsUpdateRequest));
            }

            ValidationHelper.ModelValidation(userStatisticsUpdateRequest);

            //UserStatistics? matchingUserStatistics = _userStatisticsRepository.GetStatisticsById(userStatisticsUpdateRequest.UserStatisticsId);
            List<UserStatistics> allUsers = _userStatisticsRepository.GetAllUserStatistics();
            UserStatistics? matchingUserStatistics = _userStatisticsRepository
                .GetStatisticsByIdAndUserId(userStatisticsUpdateRequest.UserStatisticsId, userStatisticsUpdateRequest.UserId);


            if (matchingUserStatistics is null)
            {
                throw new ArgumentException("Given user's statistic doesn't exist");
            }

            matchingUserStatistics.CorrectAnswersCount =
                userStatisticsUpdateRequest.CorrectAnswersCount ?? matchingUserStatistics.CorrectAnswersCount;
            matchingUserStatistics.IncorrectAnswersCount =
                userStatisticsUpdateRequest.IncorrectAnswersCount ?? matchingUserStatistics.IncorrectAnswersCount;
            matchingUserStatistics.CorrectAnswersCount += userStatisticsUpdateRequest.IsIncreaseCorrectEnteredAnswers ? 1 : 0;
            matchingUserStatistics.IncorrectAnswersCount += userStatisticsUpdateRequest.IsIncreaseIncorrectEnteredAnswers ? 1 : 0;

            _userStatisticsRepository.UpdateUserStatistics(matchingUserStatistics);

            //_userStatisticsRepository
            return matchingUserStatistics.ToUserStatisticsResponse();
        }
    }
}
