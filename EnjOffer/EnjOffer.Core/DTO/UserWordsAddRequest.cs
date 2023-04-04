using EnjOffer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class UserWordsAddRequest
    {
        [Required(ErrorMessage = "Word can't be blank")]
        public string? Word { get; set; }

        [Required(ErrorMessage = "WordTranslation can't be blank")]
        public string? WordTranslation { get; set; }

        [Required(ErrorMessage = "UserId can't be blank")]
        public Guid UserId { get; set; }

        public UserWords ToUserWords()
        {
            return new UserWords()
            {
                Word = Word, 
                WordTranslation = WordTranslation,
                UserId = UserId
            };
        }
    }
}
