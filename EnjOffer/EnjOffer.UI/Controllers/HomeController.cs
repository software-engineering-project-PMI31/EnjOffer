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
    }
}
