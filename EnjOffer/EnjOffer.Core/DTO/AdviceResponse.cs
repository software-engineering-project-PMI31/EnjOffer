using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Core.DTO
{
    public class AdviceResponse
    {
        public Guid AdviceId { get; set; }
        public int AdviceNumber { get; set; }
        public string? AdviceContent { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(AdviceResponse))
            {
                return false;
            }
            AdviceResponse advice_to_compare = (AdviceResponse)obj;
            return AdviceId == advice_to_compare.AdviceId &&
                AdviceNumber == advice_to_compare.AdviceNumber &&
                AdviceContent == advice_to_compare.AdviceContent;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3249:Classes directly extending \"object\" should not call \"base\" in \"GetHashCode\" or \"Equals\"", Justification = "<Pending>")]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class AdviceResponseExtensions
    {
        public static AdviceResponse ToAdviceResponse(this Advice advice)
        {
            return new AdviceResponse()
            {
                AdviceId = advice.AdviceId,
                AdviceNumber = advice.AdviceNumber,
                AdviceContent = advice.AdviceContent
            };
        }
    }
}
