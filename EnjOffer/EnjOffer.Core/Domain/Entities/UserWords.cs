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
        public string? UserWord { get; set; }
        public string? UserWordTranslation { get; set; }
        public int Priority { get; set; }
    }
}
