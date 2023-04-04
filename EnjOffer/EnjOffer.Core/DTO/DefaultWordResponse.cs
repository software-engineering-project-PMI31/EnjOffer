using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Core.DTO
{
    public class DefaultWordResponse
    {
        public Guid DefaultWordId { get; set; }

        public string? Word { get; set; }

        public string? WordTranslation { get; set; }

        public string? ImageSrc { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(DefaultWordResponse))
            {
                return false;
            }
            DefaultWordResponse defaultWord_to_compare = (DefaultWordResponse) obj;
            return DefaultWordId == defaultWord_to_compare.DefaultWordId &&
                Word == defaultWord_to_compare.Word &&
                WordTranslation == defaultWord_to_compare.WordTranslation &&
                ImageSrc == defaultWord_to_compare.ImageSrc;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3249:Classes directly extending \"object\" should not call \"base\" in \"GetHashCode\" or \"Equals\"", Justification = "<Pending>")]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class DefaultWordResponseExtensions
    {
        public static DefaultWordResponse ToDefaultWordResponse(this DefaultWords defaultWords)
        {
            return new DefaultWordResponse()
            {
                DefaultWordId = defaultWords.DefaultWordId,
                Word = defaultWords.Word,
                WordTranslation = defaultWords.WordTranslation,
                ImageSrc = defaultWords.ImageSrc
            };
        }
    }
}
