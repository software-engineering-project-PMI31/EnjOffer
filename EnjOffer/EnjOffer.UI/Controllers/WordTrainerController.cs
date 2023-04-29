using EnjOffer.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnjOffer.UI.Controllers
{
    public class WordTrainerController : Controller
    {
        private readonly IUserWordsService _userWordsService;

        public WordTrainerController(IUserWordsService userWordsService)
        {
            _userWordsService = userWordsService;
        }

        [Route("word-trainer")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //Url: persons/create
        [Route("[action]")]
        public IActionResult Create(UserWordsAddRequest userWordAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(userWordAddRequest);
            }

            //call the service method
            UserWordsResponse userWords = _userWordsService.AddUserWord(userWordAddRequest);

            //navigate to Index() action method (it makes another get request to "persons/index"
            return RedirectToAction("Index", "Persons");
        }
    }
}
