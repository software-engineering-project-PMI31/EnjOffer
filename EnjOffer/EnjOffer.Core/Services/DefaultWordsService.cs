using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.Helpers;

namespace EnjOffer.Core.Services
{
    public class DefaultWordsService : IDefaultWordsService
    {
        private readonly List<DefaultWords> _defaultWords;
        private readonly List<UsersDefaultWords> _usersDefaultWords;
        private readonly List<Users> _users;
        public DefaultWordsService()
        {
            _defaultWords = new List<DefaultWords>();
            _usersDefaultWords = new List<UsersDefaultWords>();
            _users = new List<Users>();
        } 
        public DefaultWordResponse AddDefaultWord(DefaultWordAddRequest? defaultWordAddRequest)
        {
            //Validation
            if (defaultWordAddRequest is null)
            {
                throw new ArgumentNullException(nameof(defaultWordAddRequest));
            }

            ValidationHelper.ModelValidation(defaultWordAddRequest);


            if (_defaultWords.Any(temp => temp.Word == defaultWordAddRequest.Word && temp.WordTranslation == defaultWordAddRequest.WordTranslation))
            {
                throw new ArgumentException("This word and translation already exist", nameof(defaultWordAddRequest));
            }

            //Convert object from DefaultWordAddRequest to DefaultWord type
            DefaultWords? defaultWord = defaultWordAddRequest.ToDefaultWords();

            //Generate DefaultWordId
            defaultWord.DefaultWordId = Guid.NewGuid();

            //Add default word into _defaultWords
            _defaultWords.Add(defaultWord);

            foreach (Users user in _users)
            {
                UsersDefaultWords userDefaultWord = new UsersDefaultWords()
                {
                    UserId = user.UserId,
                    DefaultWordId = defaultWord.DefaultWordId,
                    LastTimeEntered = null,
                    CorrectEnteredCount = 0,
                    IncorrectEnteredCount = 0

                };

                _usersDefaultWords.Add(userDefaultWord);
            }

            return defaultWord.ToDefaultWordResponse();
        }

        public List<DefaultWordResponse> GetAllDefaultWords()
        {
            return _defaultWords.Select(defaultWord => defaultWord.ToDefaultWordResponse()).ToList();
        }

        public DefaultWordResponse? GetDefaultWordById(Guid? defaultWordId)
        {
            if (defaultWordId is null)
            {
                return null;
            }

            DefaultWords? defaultWord_response_from_list = _defaultWords.FirstOrDefault
                (temp => temp.DefaultWordId == defaultWordId);

            return defaultWord_response_from_list?.ToDefaultWordResponse() ?? null;
        }

        public bool DeleteDefaultWord(Guid? defaultWordId)
        {
            if (defaultWordId is null)
            {
                throw new ArgumentNullException(nameof(defaultWordId));
            }

            DefaultWords? defaultWord = _defaultWords.FirstOrDefault(temp => temp.DefaultWordId == defaultWordId);

            if (defaultWord is null)
            {
                return false;
            }

            _defaultWords.RemoveAll(temp => temp.DefaultWordId == defaultWordId);

            return true;
        }
    }
}
