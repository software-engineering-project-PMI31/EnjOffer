using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.DTO;
using EnjOffer.Core.Helpers;
using EnjOffer.Core.ServiceContracts;

namespace EnjOffer.Core.Services
{
    public class UserWordsService : IUserWordsService
    {
        private readonly List<UserWords> _userWords;

        public UserWordsService()
        {
            _userWords = new List<UserWords>();
        }

        public double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd)
        {
            return (lastTimeEntered is not null) ? ((double)correctlyEntered / (incorrectlyEntererd +
                correctlyEntered) * (1 - Math.Exp(-(double)(DateTime.Now - lastTimeEntered).Value.Hours / 3))) : 1;
        }

        public UserWordsResponse AddUserWord(UserWordsAddRequest? userWordAddRequest)
        {
            if (userWordAddRequest is null)
            {
                throw new ArgumentNullException(nameof(userWordAddRequest));
            }

            ValidationHelper.ModelValidation(userWordAddRequest);

            if (_userWords.Any(temp => temp.Word == userWordAddRequest.Word &&
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

            _userWords.Add(userWord);

            return userWord.ToUserWordsResponse(this);
        }

        public List<UserWordsResponse> GetAllUserWords()
        {
            return _userWords.Select(userWord => userWord.ToUserWordsResponse(this)).ToList();
        }

        public UserWordsResponse? GetUserWordById(Guid? userWordId)
        {
            if (userWordId is null)
            {
                return null;
            }

            UserWords? userWord_response_from_list = _userWords.FirstOrDefault
                (temp => temp.UserWordId == userWordId);

            return userWord_response_from_list?.ToUserWordsResponse(this) ?? null;
        }

        public bool DeleteUserWord(Guid? userWordId)
        {
            if (userWordId is null)
            {
                throw new ArgumentNullException(nameof(userWordId));
            }

            UserWords? userWord = _userWords.FirstOrDefault(temp => temp.UserWordId == userWordId);

            if (userWord is null)
            {
                return false;
            }

            _userWords.RemoveAll(temp => temp.UserWordId == userWordId);

            return true;
        }

        public List<UserWordsResponse> GetUserWordsByDate(DateTime? dateTime)
        {
            if (dateTime is null)
            {
                return _userWords.Select(temp => temp.ToUserWordsResponse(this))
                    .Where(temp => temp.LastTimeEntered is null).ToList();
            }

            return _userWords.Select(temp => temp.ToUserWordsResponse(this))
                .Where(temp => temp.LastTimeEntered is not null &&
                temp.LastTimeEntered.Value.Date == dateTime.Value.Date).ToList();
        }

        public UserWordsResponse UpdateUserWord(UserWordsUpdateRequest? userWordsUpdateRequest)
        {
            if (userWordsUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(userWordsUpdateRequest));
            }

            ValidationHelper.ModelValidation(userWordsUpdateRequest);

            UserWords? matchingUserWord = _userWords.FirstOrDefault(temp => temp.UserWordId == userWordsUpdateRequest.UserWordId);
            if (matchingUserWord is null)
            {
                throw new ArgumentException("Given user's word doesn't exist");
            }

            matchingUserWord.LastTimeEntered = userWordsUpdateRequest.LastTimeEntered ?? matchingUserWord.LastTimeEntered;
            matchingUserWord.CorrectEnteredCount += userWordsUpdateRequest.IsIncreaseCorrectEnteredCount ? 1 : 0;
            matchingUserWord.IncorrectEnteredCount += userWordsUpdateRequest.IsIncreaseIncorrectEnteredCount ? 1 : 0;

            return matchingUserWord.ToUserWordsResponse(this);
        }
    }
}
