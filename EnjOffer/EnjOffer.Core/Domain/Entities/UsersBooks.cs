using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.Entities
{
    public class UsersBooks
    {
        public Guid UserId { get; set; }
        public Users? User { get; set; }
        public Guid BookId { get; set; }
        public Books? Book { get; set; }
    }
}
