using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.Entities
{
    public class UserWords
    {
        public Guid UserWordId { get; set; }
        public string? Word { get; set; }
        public string? WordTranslation { get; set; }
        public DateTime? LastTimeEntered { get; set; }
        public int CorrectEnteredCount { get; set; }
        public int IncorrectEnteredCount { get; set; }
        public int Priority { get; set; }
        public Guid UserId { get; set; }
        public Users? User { get; set; }
    }
}
