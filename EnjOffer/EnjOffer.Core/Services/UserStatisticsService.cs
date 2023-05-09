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
                _userStatisticsRepository.GetUserStatisticsByDate(userStatisticsAddRequest.AnswerDate.Value.Date) is not null)
            {
                /*throw new ArgumentException("This statistic record with such date already exist",
                    nameof(userStatisticsAddRequest));*/
                UserStatisticsUpdateRequest userStatisticsUpdateRequest = new UserStatisticsUpdateRequest()
                {
                    UserStatisticsId = _userStatisticsRepository
                        .GetUserStatisticsByDate(userStatisticsAddRequest.AnswerDate.Value.Date)!.UserStatisticsId,
                    AnswerDate = userStatisticsAddRequest.AnswerDate,
                    IsIncreaseCorrectEnteredAnswers = userStatisticsAddRequest.IsIncreaseCorrectEnteredAnswers,
                    IsIncreaseIncorrectEnteredAnswers = userStatisticsAddRequest.IsIncreaseIncorrectEnteredAnswers,
                    CorrectAnswersCount = userStatisticsAddRequest.CorrectAnswersCount,
                    IncorrectAnswersCount = userStatisticsAddRequest.IncorrectAnswersCount
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

        public List<UserStatisticsResponse> GetAllUserStatistics()
        {
            return _userStatisticsRepository.GetAllUserStatistics()
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

        public UserStatisticsResponse? GetUserStatisticsById(Guid? userStatisticsId)
        {
            if (userStatisticsId is null)
            {
                return null;
            }

            UserStatistics? userStatistics_response_from_list = _userStatisticsRepository.GetStatisticsById(userStatisticsId);

            return userStatistics_response_from_list?.ToUserStatisticsResponse() ?? null;
        }

        public UserStatisticsResponse UpdateUserStatistics(UserStatisticsUpdateRequest? userStatisticsUpdateRequest)
        {
            if (userStatisticsUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(userStatisticsUpdateRequest));
            }

            ValidationHelper.ModelValidation(userStatisticsUpdateRequest);

            UserStatistics? matchingUserStatistics = _userStatisticsRepository.GetStatisticsById(userStatisticsUpdateRequest.UserStatisticsId);
            if (matchingUserStatistics is null)
            {
                throw new ArgumentException("Given user's word doesn't exist");
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
