using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnjOffer.UI.Controllers
{
    [AllowAnonymous]
    public class LibraryController : Controller
    {
        private readonly IUserWordsService _userWordsService;

        public LibraryController(IUserWordsService userWordsService)
        {
            _userWordsService = userWordsService;
        }

        [Route("/library")]
        public IActionResult IndexLibrary()
        {
            return View();
        }
    }
}
