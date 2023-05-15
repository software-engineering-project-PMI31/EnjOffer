using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Infrastructure.Repositories
{
    public class AdviceRepository : IAdviceRepository
    {
        private readonly EnjOfferDbContext _db;

        public AdviceRepository(EnjOfferDbContext db)
        {
            _db = db;
        }

        public List<Advice>? GetAllAdvice()
        {
            return _db.Advice?.ToList();
        }
    }
}
