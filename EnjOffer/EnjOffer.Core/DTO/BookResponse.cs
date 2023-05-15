using EnjOffer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class BookResponse
    {
        public Guid BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Content { get; set; }
        public string? ImageSrc { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(BookResponse))
            {
                return false;
            }
            BookResponse book_to_compare = (BookResponse)obj;
            return BookId == book_to_compare.BookId &&
                Title == book_to_compare.Title &&
                Description == book_to_compare.Description &&
                Author == book_to_compare.Author &&
                Content == book_to_compare.Content &&
                ImageSrc == book_to_compare.ImageSrc;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3249:Classes directly extending \"object\" should not call \"base\" in \"GetHashCode\" or \"Equals\"", Justification = "<Pending>")]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class BookResponseExtensions
    {
        public static BookResponse ToBookResponse(this Books book)
        {
            return new BookResponse()
            {
                BookId = book.BookId,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                Content = book.Content,
                ImageSrc = book.ImageSrc
            };
        }
    }
}
