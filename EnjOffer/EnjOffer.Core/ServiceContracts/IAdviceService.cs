using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.DTO;

namespace EnjOffer.Core.ServiceContracts
{
    public interface IAdviceService
    {
        List<AdviceResponse>? GetAllAdvice();
    }
}
