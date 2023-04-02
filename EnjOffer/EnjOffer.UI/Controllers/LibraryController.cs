using Microsoft.AspNetCore.Mvc;

namespace EnjOffer.UI.Controllers
{
    public class LibraryController : Controller
    {
        [Route("/library")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
