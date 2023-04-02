using Microsoft.AspNetCore.Mvc;

namespace EnjOffer.UI.Controllers
{
    public class BookReaderController : Controller
    {
        [Route("/book-reader")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
