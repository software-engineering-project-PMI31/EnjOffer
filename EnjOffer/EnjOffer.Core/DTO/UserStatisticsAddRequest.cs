using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Core.DTO
{
    public class UserStatisticsAddRequest
    {
        public DateTime? AnswerDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = $"{nameof(IncorrectAnswersCount)} can't be less than 0")]
        public int? CorrectAnswersCount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = $"{nameof(IncorrectAnswersCount)} can't be less than 0")]
        public int? IncorrectAnswersCount { get; set; }

        public bool IsIncreaseCorrectEnteredAnswers { get; set; }

        public bool IsIncreaseIncorrectEnteredAnswers { get; set; }

        [Required(ErrorMessage = $"{nameof(UserId)} can't be blank")]
        public Guid? UserId { get; set; }

        public UserStatistics ToUserStatistics()
        {
            return new UserStatistics()
            {
                AnswerDate = AnswerDate,
                UserId = UserId
            };
        }
    }
}
