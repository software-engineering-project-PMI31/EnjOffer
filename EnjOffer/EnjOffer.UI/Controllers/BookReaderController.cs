using EnjOffer.Core.Domain.IdentityEntities;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnjOffer.UI.Controllers
{
    public class BookReaderController : Controller
    {
        private readonly IUserWordsService _userWordsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookReaderController(IUserWordsService userWordsService, UserManager<ApplicationUser> userManager)
        {
            _userWordsService = userWordsService;
            _userManager = userManager;
        }

        [Route("/book-reader")]
        public async Task<IActionResult> IndexBookReader()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user.Id;
            return View();
        }

        [HttpPost]
        //Url: persons/create
        [Route("/book-reader/add-user-word")]
        public IActionResult AddWord(UserWordsAddRequest userWordAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(userWordAddRequest);
            }

            try
            {
                //call the service method
                UserWordsResponse userWords = _userWordsService.AddUserWord(userWordAddRequest);

                return RedirectToAction("IndexBookReader", "BookReader");
            }
            catch (Exception)
            {
                return BadRequest("Invalid inputs");
            }
        }
    }
}
