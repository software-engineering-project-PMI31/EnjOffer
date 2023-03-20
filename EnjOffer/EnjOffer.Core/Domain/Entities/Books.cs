using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.Entities
{
    public class Books
    {
        public Guid BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Content { get; set; }
        public string? ImageSrc { get; set; }

        public ICollection<Users>? Users { get; set; }
        public ICollection<UsersBooks>? UsersBooks { get; set; }
    }
}
