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
    public class WordsService : IWordsService
    {
        private readonly IDefaultWordsRepository _defaultWordsRepository;
        private readonly IUsersDefaultWordsRepository _userDefaultWordsRepository;
        private readonly IUserWordsRepository _userWordsRepository;
        private readonly IUsersDefaultWordsService _userDefaultWordsService;
        private readonly IUserWordsService _userWordsService;
        private readonly IDefaultWordsService _defaultWordsService;

        public WordsService(IDefaultWordsRepository defaultWordsRepository,
            IUsersDefaultWordsRepository userDefaultWordsRepository, IUserWordsRepository userWordsRepository,
            IUsersDefaultWordsService userDefaultWordsService,
            IUserWordsService userWordsService, IDefaultWordsService defaultWordsService)
        {
            _defaultWordsRepository = defaultWordsRepository;
            _userDefaultWordsRepository = userDefaultWordsRepository;
            _userWordsRepository = userWordsRepository;
            _userDefaultWordsService = userDefaultWordsService;
            _userWordsService = userWordsService;
            _defaultWordsService = defaultWordsService;
        }

        public bool CheckWord(string word)
        {
            return word is not null && word == GetWordToCheck().Word;
        }

        public List<WordsResponse> GetWordsSortedByPriority(IEnumerable<WordsResponse> userWordsDefaultWords)
        {
            return userWordsDefaultWords.OrderByDescending(temp => temp.Priority).ToList();
        }

        public IEnumerable<WordsResponse> JoinDefaultWords()
        {
            List<DefaultWords> defaultWords = _defaultWordsRepository.GetAllDefaultWords();
            List<UsersDefaultWords> usersDefaultWords = _userDefaultWordsRepository.GetAllUserDefaultWords();

            return defaultWords.Join(usersDefaultWords, defaultWord => defaultWord.DefaultWordId,
                usersDefaultWord => usersDefaultWord.DefaultWordId, (defaultWord, usersDefaultWord) =>
                new WordsResponse()
                {
                    DefaultWordId = defaultWord.DefaultWordId,
                    UserId = usersDefaultWord.UserId,
                    Word = defaultWord.Word,
                    WordTranslation = defaultWord.WordTranslation,
                    LastTimeEntered = usersDefaultWord.LastTimeEntered,
                    CorrectEnteredCount = usersDefaultWord.CorrectEnteredCount,
                    IncorrectEnteredCount = usersDefaultWord.IncorrectEnteredCount,
                    Priority = _userDefaultWordsService.GetPriority(usersDefaultWord.LastTimeEntered,
                        usersDefaultWord.CorrectEnteredCount, usersDefaultWord.IncorrectEnteredCount)
                });
        }

        public IEnumerable<WordsResponse> JoinDefaultWords(List<DefaultWordResponse> defaultWords,
            List<UsersDefaultWordsResponse> usersDefaultWords)
        {
                return defaultWords.Join(usersDefaultWords, defaultWord => defaultWord.DefaultWordId,
                    usersDefaultWord => usersDefaultWord.DefaultWordId, (defaultWord, usersDefaultWord) =>
                    new WordsResponse()
                    {
                        DefaultWordId = defaultWord.DefaultWordId,
                        UserId = usersDefaultWord.UserId,
                        Word = defaultWord.Word,
                        WordTranslation = defaultWord.WordTranslation,
                        LastTimeEntered = usersDefaultWord.LastTimeEntered,
                        CorrectEnteredCount = usersDefaultWord.CorrectEnteredCount,
                        IncorrectEnteredCount = usersDefaultWord.IncorrectEnteredCount,
                        Priority = _userDefaultWordsService.GetPriority(usersDefaultWord.LastTimeEntered,
                            usersDefaultWord.CorrectEnteredCount, usersDefaultWord.IncorrectEnteredCount)
                    });
        }

        public WordsResponse GetWordToCheck()
        {
            List<UserWords> userWords = _userWordsRepository.GetAllUserWords();
            List<UserWordsResponse> userWordsResponses = _userWordsService.GetAllUserWords();
            List<DefaultWordResponse> defaultWordsResponses = _defaultWordsService.GetAllDefaultWords();
            List<UsersDefaultWordsResponse> usersDefaultWordsResponses = _userDefaultWordsService.GetAllUserDefaultWords();

            IEnumerable<WordsResponse> joined = JoinDefaultWords(defaultWordsResponses, usersDefaultWordsResponses)
                .Where(temp => temp.UserId == Guid.Parse("409855c0-8d3a-467e-8edd-87593f4a25cc"));

            /*IEnumerable<WordsResponse> joined = defaultWords.Join(usersDefaultWords, defaultWord => defaultWord.DefaultWordId,
                usersDefaultWord => usersDefaultWord.DefaultWordId, (defaultWord, usersDefaultWord) =>
                new WordsResponse()
                {
                    DefaultWordId = defaultWord.DefaultWordId,
                    UserId = usersDefaultWord.UserId,
                    Word = defaultWord.Word,
                    WordTranslation = defaultWord.WordTranslation,
                    LastTimeEntered = usersDefaultWord.LastTimeEntered,
                    CorrectEnteredCount = usersDefaultWord.CorrectEnteredCount,
                    IncorrectEnteredCount = usersDefaultWord.IncorrectEnteredCount,
                    Priority = _userDefaultWordsService.GetPriority(usersDefaultWord.LastTimeEntered,
                        usersDefaultWord.CorrectEnteredCount, usersDefaultWord.IncorrectEnteredCount)
                }).Where(temp => temp.UserId == Guid.Parse("409855c0-8d3a-467e-8edd-87593f4a25cc"));*/

            List<WordsResponse> userWordsToWords = userWords.Select(temp => temp.ToWordsResponse(_userWordsService))
                .ToList();

            var mergedWords = joined.Concat(userWordsToWords);

            var sortedWords = GetWordsSortedByPriority(mergedWords);

            WordsResponse wordToCheck = sortedWords.ElementAt(0);

            return wordToCheck;
        }

        public WordsResponse GetNextWordToCheck()
        {
            /*List<UserWords> userWords = _userWordsRepository.GetAllUserWords();
            List<DefaultWords> defaultWords = _defaultWordsRepository.GetAllDefaultWords();
            List<UsersDefaultWords> usersDefaultWords = _userDefaultWordsRepository.GetAllUserDefaultWords();*/
            List<UserWords> userWords = _userWordsRepository.GetAllUserWords();
            List<UserWordsResponse> userWordsResponses = _userWordsService.GetAllUserWords();
            List<DefaultWordResponse> defaultWordsResponses = _defaultWordsService.GetAllDefaultWords();
            List<UsersDefaultWordsResponse> usersDefaultWordsResponses = _userDefaultWordsService.GetAllUserDefaultWords();

            IEnumerable<WordsResponse> joined = JoinDefaultWords(defaultWordsResponses, usersDefaultWordsResponses)
                .Where(temp => temp.UserId == Guid.Parse("409855c0-8d3a-467e-8edd-87593f4a25cc"));

            List<WordsResponse> userWordsToWords = userWords.Select(temp => temp.ToWordsResponse(_userWordsService))
                .ToList();

            var mergedWords = joined.Concat(userWordsToWords);

            var sortedWords = GetWordsSortedByPriority(mergedWords);

            WordsResponse wordToCheck = sortedWords.ElementAt(1);

            return wordToCheck;
        }

        public WordsResponse UpdateWord(WordsUpdateRequest? wordUpdateRequest)
        {
            if (wordUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(wordUpdateRequest));
            }

            ValidationHelper.ModelValidation(wordUpdateRequest);

            if (wordUpdateRequest.DefaultWordId is null)
            {
                UserWords? matchingWord = _userWordsRepository.GetUserWordById(wordUpdateRequest.UserWordId);
                if (matchingWord is null)
                {
                    throw new ArgumentException("Given user's word doesn't exist");
                }

                matchingWord.LastTimeEntered = wordUpdateRequest.LastTimeEntered ?? DateTime.Now;
                matchingWord.CorrectEnteredCount = wordUpdateRequest.CorrectEnteredCount ?? matchingWord.CorrectEnteredCount;
                matchingWord.IncorrectEnteredCount = wordUpdateRequest.IncorrectEnteredCount ?? matchingWord.IncorrectEnteredCount;
                matchingWord.CorrectEnteredCount += wordUpdateRequest.IsIncreaseCorrectEnteredCount ? 1 : 0;
                matchingWord.IncorrectEnteredCount += wordUpdateRequest.IsIncreaseIncorrectEnteredCount ? 1 : 0;

                _userWordsRepository.UpdateUserWord(matchingWord);

                //_userWordsRepository
                return matchingWord.ToWordsResponse(_userWordsService);
            }

            if (wordUpdateRequest.UserWordId is null)
            {
                UsersDefaultWords? matchingWord = _userDefaultWordsRepository.GetUserDefaultWordById(wordUpdateRequest.DefaultWordId, wordUpdateRequest.UserId);
                if (matchingWord is null)
                {
                    throw new ArgumentException("Given default word doesn't exist");
                }

                matchingWord.LastTimeEntered = wordUpdateRequest.LastTimeEntered ?? DateTime.Now;
                matchingWord.CorrectEnteredCount = wordUpdateRequest.CorrectEnteredCount ?? matchingWord.CorrectEnteredCount;
                matchingWord.IncorrectEnteredCount = wordUpdateRequest.IncorrectEnteredCount ?? matchingWord.IncorrectEnteredCount;
                matchingWord.CorrectEnteredCount += wordUpdateRequest.IsIncreaseCorrectEnteredCount ? 1 : 0;
                matchingWord.IncorrectEnteredCount += wordUpdateRequest.IsIncreaseIncorrectEnteredCount ? 1 : 0;

                _userDefaultWordsRepository.UpdateUserDefaultWord(matchingWord);

                //_userWordsRepository
                return matchingWord.ToWordsResponse(_userDefaultWordsService, this);
            }

            throw new ArgumentException("Nor UserWord, nor DefaultWord updated", nameof(wordUpdateRequest));
        }
    }
}
