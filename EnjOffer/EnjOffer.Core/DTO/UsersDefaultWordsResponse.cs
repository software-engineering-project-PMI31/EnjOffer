using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class UsersDefaultWordsResponse
    {
        public Guid UserId { get; set; }
        public Guid DefaultWordId { get; set; }
        public DateTime? LastTimeEntered { get; set; }
        public int CorrectEnteredCount { get; set; }
        public int IncorrectEnteredCount { get; set; }
        public double Priority { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != typeof(UsersDefaultWordsResponse))
            {
                return false;
            }
            UsersDefaultWordsResponse userDefaultWord_to_compare = (UsersDefaultWordsResponse)obj;
            return UserId == userDefaultWord_to_compare.UserId &&
                DefaultWordId == userDefaultWord_to_compare.DefaultWordId &&
                LastTimeEntered == userDefaultWord_to_compare.LastTimeEntered &&
                CorrectEnteredCount == userDefaultWord_to_compare.CorrectEnteredCount &&
                IncorrectEnteredCount == userDefaultWord_to_compare.IncorrectEnteredCount &&
                Priority == userDefaultWord_to_compare.Priority;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3249:Classes directly extending \"object\" should not call \"base\" in \"GetHashCode\" or \"Equals\"", Justification = "<Pending>")]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public static class UserDefaultWordsResponseExtensions
    {
        public static UsersDefaultWordsResponse ToUserDefaultWordsResponse(this UsersDefaultWords userDefaultWords, IUsersDefaultWordsService userDefaultWordsService)
        {
            return new UsersDefaultWordsResponse()
            {
                UserId = userDefaultWords.UserId,
                DefaultWordId = userDefaultWords.DefaultWordId,
                LastTimeEntered = userDefaultWords.LastTimeEntered,
                CorrectEnteredCount = userDefaultWords.CorrectEnteredCount,
                IncorrectEnteredCount = userDefaultWords.IncorrectEnteredCount,
                Priority = userDefaultWordsService.GetPriority(userDefaultWords.LastTimeEntered, userDefaultWords.CorrectEnteredCount, userDefaultWords.IncorrectEnteredCount)
            };
        }
    }
}
