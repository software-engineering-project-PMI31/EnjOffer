using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Core.Services
{
    public class DefaultWordsService : IDefaultWordsService
    {
        private readonly List<DefaultWords> _defaultWords;
        public DefaultWordsService()
        {
            _defaultWords = new List<DefaultWords>();
        } 
        public DefaultWordResponse AddDefaultWord(DefaultWordAddRequest? defaultWordAddRequest)
        {
            //Validation
            if (defaultWordAddRequest is null)
            {
                throw new ArgumentNullException(nameof(defaultWordAddRequest));
            }

            if (defaultWordAddRequest.Word is null)
            {
                throw new ArgumentException(nameof(defaultWordAddRequest.Word));
            }

            if (defaultWordAddRequest.WordTranslation is null)
            {
                throw new ArgumentException(nameof(defaultWordAddRequest.WordTranslation));
            }

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
    }
}
