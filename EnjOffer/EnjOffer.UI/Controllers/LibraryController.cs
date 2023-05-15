using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnjOffer.UI.Controllers
{
    [AllowAnonymous]
    public class LibraryController : Controller
    {
        private readonly IUserWordsService _userWordsService;
        private readonly IBooksService _booksService;

        public LibraryController(IUserWordsService userWordsService, IBooksService booksService)
        {
            _userWordsService = userWordsService;
            _booksService = booksService;
        }

        [Route("/library")]
        [HttpGet]
        public IActionResult IndexLibrary()
        {
            ViewBag.Books = _booksService.GetAllBooks();
            BookResponse? book = _booksService.GetFirstSelectedBook();

            HttpContext.Session.Remove("Book");

            return View(book);
        }

        [Route("/library")]
        [HttpPost]
        public IActionResult IndexLibraryGetBookToShow(Guid? bookId)
        {
            ViewBag.Books = _booksService.GetAllBooks();

            BookResponse? book = _booksService.GetBookById(bookId);

            return View("IndexLibrary", book);
        }

        [Route("/book-reader")]
        [HttpPost]
        public IActionResult OpenBookReader(Guid? BookId)
        {
            BookResponse? book = _booksService.GetBookById(BookId);
            string serializedBook = JsonConvert.SerializeObject(book);
            HttpContext.Session.SetString("Book", serializedBook);

            return RedirectToAction("IndexBookReader", "BookReader");
        }
    }
}
