using Microsoft.AspNetCore.Mvc;

namespace EnjOffer.UI.Controllers
{
    public class WordTrainerController : Controller
    {
        [Route("word-trainer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
