using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.DTO;
using EnjOffer.Core.Helpers;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Domain.RepositoryContracts;

namespace EnjOffer.Core.Services
{
    public class UserWordsService : IUserWordsService
    {
        private readonly IUserWordsRepository _userWordsRepository;

        public UserWordsService(IUserWordsRepository userWords)
        {
            _userWordsRepository = userWords;
        }

        public double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd)
        {
            return (lastTimeEntered is not null)
                ? (double)correctlyEntered / (incorrectlyEntererd +
                correctlyEntered) * (1 - Math.Exp(-(double)(DateTime.Now - ((DateTime)lastTimeEntered)).TotalHours / 3))
                : 1;
        }

        public UserWordsResponse AddUserWord(UserWordsAddRequest? userWordAddRequest)
        {
            if (userWordAddRequest is null)
            {
                throw new ArgumentNullException(nameof(userWordAddRequest));
            }

            ValidationHelper.ModelValidation(userWordAddRequest);

            if (_userWordsRepository.GetAllUserWords().Any(temp => temp.Word == userWordAddRequest.Word &&
            temp.WordTranslation == userWordAddRequest.WordTranslation &&
            temp.UserId == userWordAddRequest.UserId))
            {
                throw new ArgumentException("This user already exists", nameof(userWordAddRequest));
            }

            //Convert userWordAddRequest to UserWords type
            UserWords userWord = userWordAddRequest.ToUserWords();

            //Generate UserWordId
            userWord.UserWordId = Guid.NewGuid();

            //Generate Datetime and default correct and incorrect answers
            userWord.LastTimeEntered = null;
            userWord.CorrectEnteredCount = 0;
            userWord.IncorrectEnteredCount = 0;

            _userWordsRepository.AddUserWord(userWord);

            return userWord.ToUserWordsResponse(this);
        }

        public List<UserWordsResponse> GetAllUserWords()
        {
            return _userWordsRepository.GetAllUserWords().Select(userWord => userWord.ToUserWordsResponse(this)).ToList();
        }

        public UserWordsResponse? GetUserWordById(Guid? userWordId)
        {
            if (userWordId is null)
            {
                return null;
            }

            UserWords? userWord_response_from_list = _userWordsRepository.GetUserWordById(userWordId);

            return userWord_response_from_list?.ToUserWordsResponse(this) ?? null;
        }

        public bool DeleteUserWord(Guid? userWordId)
        {
            if (userWordId is null)
            {
                throw new ArgumentNullException(nameof(userWordId));
            }

            UserWords? userWord = _userWordsRepository.GetUserWordById(userWordId);

            if (userWord is null)
            {
                return false;
            }

            _userWordsRepository.DeleteUserWord(userWordId);

            return true;
        }

        public List<UserWordsResponse> GetUserWordsByDate(DateTime? dateTime)
        {
            /*if (dateTime is null)
            {
                *//*return _userWordsRepository.GetAllUserWords().Select(temp => temp.ToUserWordsResponse(this))
                    .Where(temp => temp.LastTimeEntered is null).ToList();*//*
               
            }*/
            return _userWordsRepository.GetUserWordsByDate(dateTime).Select(temp => temp.ToUserWordsResponse(this)).ToList();
            /*return _userWordsRepository.GetAllUserWords().Select(temp => temp.ToUserWordsResponse(this))
                .Where(temp => temp.LastTimeEntered is not null &&
                temp.LastTimeEntered.Value.Date == dateTime.Value.Date).ToList();*/
        }

        public UserWordsResponse UpdateUserWord(UserWordsUpdateRequest? userWordsUpdateRequest)
        {
            if (userWordsUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(userWordsUpdateRequest));
            }

            ValidationHelper.ModelValidation(userWordsUpdateRequest);

            UserWords? matchingUserWord = _userWordsRepository.GetUserWordById(userWordsUpdateRequest.UserWordId);
            if (matchingUserWord is null)
            {
                throw new ArgumentException("Given user's word doesn't exist");
            }

            matchingUserWord.LastTimeEntered = userWordsUpdateRequest.LastTimeEntered ?? matchingUserWord.LastTimeEntered;
            matchingUserWord.CorrectEnteredCount = userWordsUpdateRequest.CorrectEnteredCount ?? matchingUserWord.CorrectEnteredCount;
            matchingUserWord.IncorrectEnteredCount = userWordsUpdateRequest.IncorrectEnteredCount ?? matchingUserWord.IncorrectEnteredCount;
            matchingUserWord.CorrectEnteredCount += userWordsUpdateRequest.IsIncreaseCorrectEnteredCount ? 1 : 0;
            matchingUserWord.IncorrectEnteredCount += userWordsUpdateRequest.IsIncreaseIncorrectEnteredCount ? 1 : 0;

            _userWordsRepository.UpdateUserWord(matchingUserWord);

            //_userWordsRepository
            return matchingUserWord.ToUserWordsResponse(this);
        }

        public List<UserWordsResponse> GetUserWordsSortedByPriority(List<UserWordsResponse> userWords)
        {
            return userWords.OrderByDescending(temp => temp.Priority).ToList();
        }
    }
}
