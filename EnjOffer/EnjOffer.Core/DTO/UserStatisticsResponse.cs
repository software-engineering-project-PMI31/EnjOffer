using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;

namespace EnjOffer.Core.DTO
{
    public class UserStatisticsResponse
    {
        public Guid UserStatisticsId { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int IncorrectAnswersCount { get; set; }
        public Guid? UserId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != typeof(UserStatisticsResponse))
            {
                return false;
            }

            UserStatisticsResponse userStatistics_to_compare = (UserStatisticsResponse)obj;
            return UserStatisticsId == userStatistics_to_compare.UserStatisticsId &&
                   AnswerDate == userStatistics_to_compare.AnswerDate &&
                   CorrectAnswersCount == userStatistics_to_compare.CorrectAnswersCount &&
                   IncorrectAnswersCount == userStatistics_to_compare.IncorrectAnswersCount &&
                   UserId == userStatistics_to_compare.UserId;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3249:Classes directly extending \"object\" should not call \"base\" in \"GetHashCode\" or \"Equals\"", Justification = "<Pending>")]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public UserStatisticsUpdateRequest ToUserStatisticsUpdateRequest()
        {
            return new UserStatisticsUpdateRequest()
            {
                UserStatisticsId = UserStatisticsId,
                CorrectAnswersCount = CorrectAnswersCount,
                IncorrectAnswersCount = IncorrectAnswersCount
            };
        }
    }

    public static class UserStatisticsResponseExtensions
    {
        public static UserStatisticsResponse ToUserStatisticsResponse(this UserStatistics userStatistics)
        {
            return new UserStatisticsResponse()
            {
                UserStatisticsId = userStatistics.UserStatisticsId,
                AnswerDate = userStatistics.AnswerDate,
                CorrectAnswersCount = userStatistics.CorrectAnswersCount,
                IncorrectAnswersCount = userStatistics.IncorrectAnswersCount,
                UserId = userStatistics.UserId
            };
        }
    }
}
