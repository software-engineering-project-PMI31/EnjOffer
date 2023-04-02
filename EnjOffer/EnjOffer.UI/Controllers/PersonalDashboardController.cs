using Microsoft.AspNetCore.Mvc;

namespace EnjOffer.UI.Controllers
{
    public class PersonalDashboardController : Controller
    {
        [Route("/personal-dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
