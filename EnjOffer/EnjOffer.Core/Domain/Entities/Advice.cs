using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.Entities
{
    public class Advice
    {
        public Guid AdviceId { get; set; }
        public int AdviceNumber { get; set; }
        public string? AdviceContent { get; set; }
        public ICollection<Users>? Users { get; set; }
        public ICollection<UsersAdvice>? UsersAdvice { get; set; }
    }
}
