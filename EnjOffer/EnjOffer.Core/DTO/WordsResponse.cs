using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class WordsResponse
    {
        public Guid? UserWordId { get; set; }
        public Guid? DefaultWordId { get; set; }
        public string? Word { get; set; }
        public string? WordTranslation { get; set; }
        public DateTime? LastTimeEntered { get; set; }
        public int CorrectEnteredCount { get; set; }
        public int IncorrectEnteredCount { get; set; }
        public string? ImageSrc { get; set; }
        public double Priority { get; set; }
        public Guid? UserId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != typeof(WordsResponse))
            {
                return false;
            }

            WordsResponse word_to_compare = (WordsResponse)obj;
            return UserWordId == word_to_compare.UserWordId &&
                DefaultWordId == word_to_compare.DefaultWordId &&
                Word == word_to_compare.Word &&
                WordTranslation == word_to_compare.WordTranslation &&
                LastTimeEntered == word_to_compare.LastTimeEntered &&
                CorrectEnteredCount == word_to_compare.CorrectEnteredCount &&
                IncorrectEnteredCount == word_to_compare.IncorrectEnteredCount &&
                Math.Round(Priority, 6) == Math.Round(word_to_compare.Priority, 6) &&
                UserId == word_to_compare.UserId &&
                string.Equals(ImageSrc, word_to_compare.ImageSrc);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3249:Classes directly extending \"object\" should not call \"base\" in \"GetHashCode\" or \"Equals\"", Justification = "<Pending>")]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public WordsUpdateRequest ToWordsUpdateRequest()
        {
            return new WordsUpdateRequest()
            {
                UserId = UserId,
                UserWordId = UserWordId,
                DefaultWordId = DefaultWordId,
                Word = Word,
                WordTranslation = WordTranslation,
                ImageSrc = ImageSrc,
                LastTimeEntered = LastTimeEntered,
                CorrectEnteredCount = CorrectEnteredCount,
                IncorrectEnteredCount = IncorrectEnteredCount
            };
        }
    }

    public static class WordsResponseExtensions
    {
        public static WordsResponse ToWordsResponse(this UserWords userWords, IUserWordsService userWordsService)
        {
            return new WordsResponse()
            {
                UserWordId = userWords.UserWordId,
                Word = userWords.Word,
                WordTranslation = userWords.WordTranslation,
                LastTimeEntered = userWords.LastTimeEntered,
                CorrectEnteredCount = userWords.CorrectEnteredCount,
                IncorrectEnteredCount = userWords.IncorrectEnteredCount,
                Priority = userWordsService.GetPriority(userWords.LastTimeEntered,
                    userWords.CorrectEnteredCount, userWords.IncorrectEnteredCount),
                UserId = userWords.UserId
            };
        }

        public static WordsResponse ToWordsResponse(this UsersDefaultWords userDefaultWords,
            IUsersDefaultWordsService userDefaultWordsService, IWordsService wordsService)
        {
            IEnumerable<WordsResponse> joinedDefaultWords = wordsService.JoinDefaultWords();

            return new WordsResponse()
            {
                UserId = userDefaultWords.UserId,
                DefaultWordId = userDefaultWords.DefaultWordId,

                Word = joinedDefaultWords.FirstOrDefault(word => word.UserWordId is null && 
                    word.UserId == userDefaultWords.UserId &&
                    word.DefaultWordId == userDefaultWords.DefaultWordId)?.Word,

                WordTranslation = joinedDefaultWords.FirstOrDefault(word => word.UserWordId is null &&
                    word.UserId == userDefaultWords.UserId &&
                    word.DefaultWordId == userDefaultWords.DefaultWordId)?.WordTranslation,

                LastTimeEntered = userDefaultWords.LastTimeEntered,
                CorrectEnteredCount = userDefaultWords.CorrectEnteredCount,
                IncorrectEnteredCount = userDefaultWords.IncorrectEnteredCount,
                Priority = userDefaultWordsService.GetPriority(userDefaultWords.LastTimeEntered,
                    userDefaultWords.CorrectEnteredCount, userDefaultWords.IncorrectEnteredCount),
            };
        }
    }
}
