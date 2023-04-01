using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Core.DTO
{
    public class DefaultWordAddRequest
    {
        public string? Word { get; set; }

        public string? WordTranslation { get; set; }

        public string? ImageSrc { get; set; }

        public DefaultWords ToDefaultWords()
        {
            return new DefaultWords() { Word = Word, WordTranslation = WordTranslation, ImageSrc = ImageSrc};
        }
    }
}
