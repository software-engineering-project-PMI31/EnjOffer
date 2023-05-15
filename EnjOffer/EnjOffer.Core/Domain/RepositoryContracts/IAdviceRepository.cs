using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Core.Domain.RepositoryContracts
{
    public interface IAdviceRepository
    {
        List<Advice>? GetAllAdvice();
    }
}
