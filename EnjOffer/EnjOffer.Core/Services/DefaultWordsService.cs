using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.Helpers;

namespace EnjOffer.Core.Services
{
    public class DefaultWordsService : IDefaultWordsService
    {
        private readonly IDefaultWordsRepository _defaultWordsRepository;

        public DefaultWordsService(IDefaultWordsRepository defaultWordsRepository)
        {
            _defaultWordsRepository = defaultWordsRepository;
        } 
        public DefaultWordResponse AddDefaultWord(DefaultWordAddRequest? defaultWordAddRequest)
        {
            //Validation
            if (defaultWordAddRequest is null)
            {
                throw new ArgumentNullException(nameof(defaultWordAddRequest));
            }

            ValidationHelper.ModelValidation(defaultWordAddRequest);

            if (defaultWordAddRequest.Word is not null &&
                defaultWordAddRequest.WordTranslation is not null &&
                _defaultWordsRepository.GetDefaultWordByWordAndTranslation(defaultWordAddRequest.Word,
                defaultWordAddRequest.WordTranslation) is not null)
            {
                throw new ArgumentException("This word and translation already exist", nameof(defaultWordAddRequest));
            }

            //Convert object from DefaultWordAddRequest to DefaultWord type
            DefaultWords? defaultWord = defaultWordAddRequest.ToDefaultWords();

            //Generate DefaultWordId
            defaultWord.DefaultWordId = Guid.NewGuid();

            //Add default word into _defaultWords
            _defaultWordsRepository.AddDefaultWord(defaultWord);

            return defaultWord.ToDefaultWordResponse();
        }

        public List<DefaultWordResponse> GetAllDefaultWords()
        {
            return _defaultWordsRepository.GetAllDefaultWords().Select(defaultWord => defaultWord.ToDefaultWordResponse()).ToList();
        }

        public DefaultWordResponse? GetDefaultWordById(Guid? defaultWordId)
        {
            if (defaultWordId is null)
            {
                return null;
            }

            DefaultWords? defaultWord_response_from_list = _defaultWordsRepository.GetDefaultWordById(defaultWordId.Value);

            return defaultWord_response_from_list?.ToDefaultWordResponse() ?? null;
        }

        public bool DeleteDefaultWord(Guid? defaultWordId)
        {
            if (defaultWordId is null)
            {
                throw new ArgumentNullException(nameof(defaultWordId));
            }

            DefaultWords? defaultWord = _defaultWordsRepository.GetDefaultWordById(defaultWordId.Value);

            if (defaultWord is null)
            {
                return false;
            }

            _defaultWordsRepository.DeleteDefaultWord(defaultWordId.Value);

            return true;
        }

        public bool DeleteDefaultWordByWordAndTranslation(string word, string wordTranslation)
        {
            if (_defaultWordsRepository.GetDefaultWordByWordAndTranslation(word, wordTranslation) is null)
            {
                throw new ArgumentNullException(nameof(word));
            }

            DefaultWords? defaultWord = _defaultWordsRepository.GetDefaultWordByWordAndTranslation(word, wordTranslation);

            if (defaultWord is null)
            {
                return false;
            }

            _defaultWordsRepository.DeleteDefaultWord(defaultWord.DefaultWordId);

            return true;
        }
    }
}
