using Microsoft.AspNetCore.Mvc;

namespace EnjOffer.UI.Controllers
{
    public class LoginPageConroller : Controller
    {
        [Route("/login")]
        public IActionResult Index()
        {
            return View();
        }
    }
}