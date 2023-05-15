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
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;

        public BooksService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }
        public List<BookResponse>? GetAllBooks()
        {
            return _booksRepository.GetAllBooks()?.Select(book => book.ToBookResponse()).ToList();
        }

        public BookResponse? GetBookById(Guid? bookId)
        {
            return _booksRepository.GetBookById(bookId)?.ToBookResponse();
        }

        public BookResponse? GetFirstSelectedBook()
        {
            return _booksRepository.GetAllBooks()?.ElementAt(0).ToBookResponse();
        }
    }
}
