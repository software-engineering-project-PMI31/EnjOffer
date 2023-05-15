using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.Helpers;

namespace EnjOffer.Core.Services
{
    public class AdviceService : IAdviceService
    {
        private readonly IAdviceRepository _adviceRepository;

        public AdviceService(IAdviceRepository adviceRepository)
        {
            _adviceRepository = adviceRepository;
        }

        public List<AdviceResponse>? GetAllAdvice()
        {
            return _adviceRepository.GetAllAdvice()?.Select(advice => advice.ToAdviceResponse()).ToList();
        }
    }
}
