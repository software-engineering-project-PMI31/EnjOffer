using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.DTO;

namespace EnjOffer.Core.ServiceContracts
{
    public interface IBooksService
    {
        List<BookResponse>? GetAllBooks();
        BookResponse? GetBookById(Guid? bookId);
        BookResponse? GetFirstSelectedBook();
    }
}
