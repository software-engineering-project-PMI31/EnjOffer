using Microsoft.AspNetCore.Mvc;

namespace EnjOffer.UI.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Header()
        {
            return View();
        }

        public IActionResult Sidebar()
        {
            return View();
        }
    }
}
