using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.Entities
{
    public class UsersDefaultWords
    {
        public Guid UserId { get; set; }
        public Users? User { get; set; }
        public Guid? DefaultWordId { get; set; }
        public DefaultWords? DefaultWord { get; set; }
        public DateTime? LastTimeEntered { get; set; }
        public int CorrectEnteredCount { get; set; }
        public int IncorrectEnteredCount { get; set; }
    }
}
