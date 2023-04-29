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
        [Required(ErrorMessage = $"{nameof(AnswerDate)} can't be blank")]
        public DateTime? AnswerDate { get; set; }

        [Required(ErrorMessage = $"{nameof(UserId)} can't be blank")]
        public Guid UserId { get; set; }

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
