using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Infrastructure.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly EnjOfferDbContext _db;

        public BooksRepository(EnjOfferDbContext db)
        {
            _db = db;
        }

        public List<Books>? GetAllBooks()
        {
            return _db.Books?.ToList();
        }

        public Books? GetBookById(Guid? bookId)
        {
            return _db.Books?.FirstOrDefault(temp => temp.BookId == bookId);
        }
    }
}
