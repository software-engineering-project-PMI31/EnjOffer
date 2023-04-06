using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;

namespace EnjOffer.Core.DTO
{
    public class UserWordsResponse
    {
        public Guid UserWordId { get; set; }
        public string? Word { get; set; }
        public string? WordTranslation { get; set; }
        public DateTime? LastTimeEntered { get; set; }
        public int CorrectEnteredCount { get; set; }
        public int IncorrectEnteredCount { get; set; }
        public double Priority { get; set; }
        public Guid? UserId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != typeof(UserWordsResponse))
            {
                return false;
            }
            UserWordsResponse userWord_to_compare = (UserWordsResponse)obj;
            return UserWordId == userWord_to_compare.UserWordId &&
                Word == userWord_to_compare.Word &&
                WordTranslation == userWord_to_compare.WordTranslation &&
                LastTimeEntered == userWord_to_compare.LastTimeEntered &&
                CorrectEnteredCount == userWord_to_compare.CorrectEnteredCount &&
                IncorrectEnteredCount == userWord_to_compare.IncorrectEnteredCount &&
                Priority == userWord_to_compare.Priority &&
                UserId == userWord_to_compare.UserId;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3249:Classes directly extending \"object\" should not call \"base\" in \"GetHashCode\" or \"Equals\"", Justification = "<Pending>")]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public UserWordsUpdateRequest ToUserWordsUpdateRequest()
        {
            return new UserWordsUpdateRequest()
            {
                UserWordId = UserWordId,
                LastTimeEntered = LastTimeEntered,
                CorrectEnteredCount = CorrectEnteredCount,
                IncorrectEnteredCount = IncorrectEnteredCount
            };
        }
    }

    public static class UserWordsResponseExtensions
    {
        public static UserWordsResponse ToUserWordsResponse(this UserWords userWords, IUserWordsService userWordsService)
        {
            return new UserWordsResponse()
            {
                UserWordId = userWords.UserWordId,
                Word = userWords.Word,
                WordTranslation = userWords.WordTranslation,
                LastTimeEntered = userWords.LastTimeEntered,
                CorrectEnteredCount = userWords.CorrectEnteredCount,
                IncorrectEnteredCount = userWords.IncorrectEnteredCount,
                Priority = userWordsService.GetPriority(userWords.LastTimeEntered, userWords.CorrectEnteredCount, userWords.IncorrectEnteredCount),
                UserId = userWords.UserId
            };
        }
    }
}
