using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.Entities
{
    public class UsersAdvice
    {
        public Guid UserId { get; set; }

        public Users? User { get; set; }

        public Guid AdviceId { get; set; }

        public Advice? Advice { get; set; }
    }
}
