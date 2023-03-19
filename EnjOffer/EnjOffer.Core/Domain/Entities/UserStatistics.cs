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
        public int CorrectAnswersCount { get; set; }
        public int IncorrectAnswersCount { get; set; }
    }
}
