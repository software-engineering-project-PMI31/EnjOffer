using Microsoft.AspNetCore.Mvc;

namespace EnjOffer.UI.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
