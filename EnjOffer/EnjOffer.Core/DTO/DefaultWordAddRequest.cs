using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Core.DTO
{
    public class DefaultWordAddRequest
    {
        [Required(ErrorMessage = "Word can't be blank")]
        public string? Word { get; set; }

        [Required(ErrorMessage = "WordTranslation can't be blank")]
        public string? WordTranslation { get; set; }

        public string? ImageSrc { get; set; }

        public DefaultWords ToDefaultWords()
        {
            return new DefaultWords() { Word = Word, WordTranslation = WordTranslation, ImageSrc = ImageSrc};
        }
    }
}
