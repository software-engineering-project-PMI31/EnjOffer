using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.Helpers;

namespace EnjOffer.Core.Services
{
    public class UsersDefaultWordsService : IUsersDefaultWordsService
    {
        private readonly IUsersDefaultWordsRepository _usersDefaultWordsRepository;
        private readonly IDefaultWordsRepository _defaultWordsRepository;

        public UsersDefaultWordsService(IUsersDefaultWordsRepository usersDefaultWordsRepository,
            IDefaultWordsRepository defaultWordsRepository)
        {
            _usersDefaultWordsRepository = usersDefaultWordsRepository;
            _defaultWordsRepository = defaultWordsRepository;
        }

        public UsersDefaultWordsResponse AddUserDefaultWord(UsersDefaultWordsAddRequest? usersDefaultWordAddRequest)
        {
            if (usersDefaultWordAddRequest is null)
            {
                throw new ArgumentNullException(nameof(usersDefaultWordAddRequest));
            }

            ValidationHelper.ModelValidation(usersDefaultWordAddRequest);

            if (_usersDefaultWordsRepository
                .GetUserDefaultWordById(usersDefaultWordAddRequest.DefaultWordId, usersDefaultWordAddRequest.UserId) is not null)
            {
                return _usersDefaultWordsRepository
                .GetUserDefaultWordById(usersDefaultWordAddRequest.DefaultWordId, usersDefaultWordAddRequest.UserId)!
                .ToUserDefaultWordsResponse(this);
                //throw new ArgumentException("This default word with such user id already exist", nameof(usersDefaultWordAddRequest));
            }

            //Convert usersDefaultWordAddRequest to UsersDefaultWords type
            UsersDefaultWords? usersDefaultWord = usersDefaultWordAddRequest.ToUsersDefaultWords();

            //Generate dId
            usersDefaultWord.DefaultWordId = usersDefaultWordAddRequest.DefaultWordId;
            usersDefaultWord.UserId = usersDefaultWordAddRequest.UserId;

            //Generate Datetime and default correct and incorrect answers
            usersDefaultWord.LastTimeEntered = null;
            usersDefaultWord.CorrectEnteredCount = 0;
            usersDefaultWord.IncorrectEnteredCount = 0;

            _usersDefaultWordsRepository.AddUserDefaultWord(usersDefaultWord);

            return usersDefaultWord.ToUserDefaultWordsResponse(this);
        }

        public bool DeleteUserDefaultWord(Guid? defaultWordId, Guid? userId)
        {
            throw new NotImplementedException();
        }

        public List<UsersDefaultWordsResponse> GetAllUserDefaultWords()
        {
            return _usersDefaultWordsRepository.GetAllUserDefaultWords()
                .Select(usersDefaultWord => usersDefaultWord.ToUserDefaultWordsResponse(this)).ToList();
        }

        public double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd)
        {
            /*const double correctlyEnteredWeight = 0.9;
            const double incorrectlyEnteredWeight = 0.1;
            double decayRate = (lastTimeEntered is not null) &&
                (DateTime.Now - ((DateTime)lastTimeEntered)).TotalHours > 200 ? 600 : 400;

            return (lastTimeEntered is not null)
                ? ((double)correctlyEntered * correctlyEnteredWeight) / ((incorrectlyEntererd * incorrectlyEnteredWeight) +
                (correctlyEntered * correctlyEnteredWeight)) * (1 - Math.Exp(-(DateTime.Now - ((DateTime)lastTimeEntered)).TotalHours / decayRate))
                : 1;*/

            /*const double correctlyEnteredWeight = 8;
            const double incorrectlyEnteredWeight = 2;
            double decayRate = (lastTimeEntered is not null) &&
                (DateTime.Now - ((DateTime)lastTimeEntered)).TotalHours > 200 ? 600 : 400;

            return (lastTimeEntered is not null) ?
                ((double)correctlyEntered * correctlyEnteredWeight) / ((incorrectlyEntererd * incorrectlyEnteredWeight) +
                (correctlyEntered * correctlyEnteredWeight)) * (1 / (1 + Math.Exp(-(DateTime.Now - ((DateTime)lastTimeEntered)).TotalHours / decayRate))) :
                1;*/
            return 1.0 / GetRepetitionInterval(correctlyEntered, incorrectlyEntererd, lastTimeEntered);
        }

        private const double a = 0.5;
        private const double b = 1.6;

        public static double GetRepetitionInterval(int numCorrect, int numIncorrect, DateTime? lastViewed)
        {
            if (numCorrect < 0 || numIncorrect < 0)
                throw new ArgumentException("Number of correct and incorrect attempts must be non-negative.");

            if (lastViewed is null)
            {
                return 1;
            }
            TimeSpan timeSinceLastViewed = DateTime.Now - (DateTime)lastViewed;
            double daysSinceLastViewed = timeSinceLastViewed.TotalMinutes;

            if (numCorrect == 0)
                return 0.0;

            double interval = 1.0;

            if (numIncorrect == 0)
                interval = 1.0;
            else if (numIncorrect == 1)
                interval = 1.0 / a;
            else
                interval = GetInterval(numCorrect, numIncorrect);

            return interval * daysSinceLastViewed;
        }

        private static double GetInterval(int numCorrect, int numIncorrect)
        {
            double interval = 1.0;
            for (int i = 1; i <= numIncorrect; i++)
                interval *= b;
            interval *= GetEaseFactor(numCorrect);
            return interval;
        }

        private static double GetEaseFactor(int numCorrect)
        {
            return 1.3 - 0.1 * numCorrect + 0.02 * numCorrect * numCorrect;
        }

        public UsersDefaultWordsResponse? GetUserDefaultWordById(Guid? defaultWordId, Guid? userId)
        {
            if (defaultWordId is null || userId is null)
            {
                return null;
            }

            UsersDefaultWords? usersDefaultWord_response_from_list = 
                _usersDefaultWordsRepository.GetUserDefaultWordById(defaultWordId, userId);

            return usersDefaultWord_response_from_list?.ToUserDefaultWordsResponse(this) ?? null;
        }

        public List<UsersDefaultWordsResponse> GetUserDefaultWordsByDate(DateTime? dateTime)
        {
            return _usersDefaultWordsRepository.GetUserDefaultWordsByDate(dateTime)
                .Select(temp => temp.ToUserDefaultWordsResponse(this)).ToList();
        }

        public List<UsersDefaultWordsResponse> GetUserDefaultWordsSortedByPriority(List<UsersDefaultWordsResponse> usersDefaultWords)
        {
            return usersDefaultWords.OrderByDescending(temp => temp.Priority).ToList();
        }

        public UsersDefaultWordsResponse UpdateUserDefaultWord(UsersDefaultWordsUpdateRequest? usersDefaultWordsUpdateRequest)
        {
            if (usersDefaultWordsUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(usersDefaultWordsUpdateRequest));
            }

            ValidationHelper.ModelValidation(usersDefaultWordsUpdateRequest);

            UsersDefaultWords? matchingUsersDefaultWord =
                _usersDefaultWordsRepository.GetUserDefaultWordById(usersDefaultWordsUpdateRequest.DefaultWordId,
                    usersDefaultWordsUpdateRequest.UserId);

            if (matchingUsersDefaultWord is null)
            {
                throw new ArgumentException("Given word with such user id doesn't exist");
            }

            matchingUsersDefaultWord.LastTimeEntered =
                usersDefaultWordsUpdateRequest.LastTimeEntered ?? DateTime.Now;
            matchingUsersDefaultWord.CorrectEnteredCount =
                usersDefaultWordsUpdateRequest.CorrectEnteredCount ?? matchingUsersDefaultWord.CorrectEnteredCount;
            matchingUsersDefaultWord.IncorrectEnteredCount =
                usersDefaultWordsUpdateRequest.IncorrectEnteredCount ?? matchingUsersDefaultWord.IncorrectEnteredCount;
            matchingUsersDefaultWord.CorrectEnteredCount +=
                usersDefaultWordsUpdateRequest.IsIncreaseCorrectEnteredCount ? 1 : 0;
            matchingUsersDefaultWord.IncorrectEnteredCount +=
                usersDefaultWordsUpdateRequest.IsIncreaseIncorrectEnteredCount ? 1 : 0;

            _usersDefaultWordsRepository.UpdateUserDefaultWord(matchingUsersDefaultWord);

            //_userWordsRepository
            return matchingUsersDefaultWord.ToUserDefaultWordsResponse(this);
        }

        //TO-DO: create interface and test
        public DefaultWordResponse? GetDefaultWordToShow()
        {
            List<UsersDefaultWordsResponse> usersDefaultWords =
                _usersDefaultWordsRepository.GetAllUserDefaultWords()
                .Select(usersDefaultWord => usersDefaultWord.ToUserDefaultWordsResponse(this)).ToList();

            UsersDefaultWordsResponse mathingDefaultWord =
                usersDefaultWords.OrderByDescending(temp => temp.Priority).ElementAt(0);

            return _defaultWordsRepository.GetAllDefaultWords()
                .Select(temp => temp.ToDefaultWordResponse())
                .FirstOrDefault(temp => temp.DefaultWordId == mathingDefaultWord.DefaultWordId);
        }
    }
}
