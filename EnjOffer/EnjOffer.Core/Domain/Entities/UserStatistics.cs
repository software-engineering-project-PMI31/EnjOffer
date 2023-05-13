using EnjOffer.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.Entities
{
    public class UserStatistics
    {
        public Guid UserStatisticsId { get; set; }
        public DateTime? AnswerDate { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int IncorrectAnswersCount { get; set; }
        /*public Guid? UserId { get; set; }
        public Users? User { get; set; }*/
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
