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
    }
}
