using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.DTO;
using EnjOffer.Core.Helpers;
using EnjOffer.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Services
{
    public class UserWordsService : IUserWordsService
    {
        private readonly List<UserWords> _userWords;

        public UserWordsService()
        {
            _userWords = new List<UserWords>();
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
            userWord.LastTimeEntered = DateTime.Now;
            userWord.CorrectEnteredCount = 0;
            userWord.IncorrectEnteredCount = 0;

            _userWords.Add(userWord);

            return userWord.ToUserWordsResponse();
        }

        public List<UserWordsResponse> GetAllUserWords()
        {
            throw new NotImplementedException();
        }
    }
}
