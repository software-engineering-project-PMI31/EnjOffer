using EnjOffer.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using EnjOffer.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace EnjOffer.UI.Controllers
{
    public class WordTrainerController : Controller
    {
        private readonly IDefaultWordsService _defaultWordsService;
        private readonly IUsersDefaultWordsService _usersDefaultWordsService;
        private readonly IUserWordsService _userWordsService;
        private readonly IWordsService _wordsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WordTrainerController(IDefaultWordsService defaultWordsService, 
            IUsersDefaultWordsService usersDefaultWordsService, IUserWordsService userWordsService,
            IWordsService wordsService, UserManager<ApplicationUser> userManager)
        {
            _defaultWordsService = defaultWordsService;
            _usersDefaultWordsService = usersDefaultWordsService;
            _userWordsService = userWordsService;
            _wordsService = wordsService;
            _userManager = userManager;
        }

        [Route("word-trainer")]
        [HttpGet]
        public async Task<IActionResult> IndexWordTrainer(WordsUpdateRequest? wordsUpdateRequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            WordsResponse wordToCheck = _wordsService.GetWordToCheck(user.Id);

            if (wordsUpdateRequest?.DefaultWordId is null && wordsUpdateRequest?.UserId is null &&
                wordsUpdateRequest?.UserWordId is null)
            {
                WordsUpdateRequest? wordsNewUpdateRequest = new WordsUpdateRequest()
                {
                    DefaultWordId = wordToCheck.DefaultWordId,
                    UserWordId = wordToCheck.UserWordId,
                    UserId = wordToCheck.UserId,
                    Word = wordToCheck.Word,
                    WordTranslation = wordToCheck.WordTranslation,
                    CorrectEnteredCount = wordToCheck.CorrectEnteredCount,
                    IncorrectEnteredCount = wordToCheck.IncorrectEnteredCount,
                    LastTimeEntered = wordToCheck.LastTimeEntered,
                    ImageSrc = wordToCheck.ImageSrc
                };

                ViewBag.IsCorrectEntered = null;

                HttpContext.Session.Remove("Book");

                ViewBag.Placeholder = HttpContext.Session.GetString("Placeholder");
                HttpContext.Session.Remove("Placeholder");

                return View(wordsNewUpdateRequest);
            }
            else
            {
                ViewBag.IsCorrectEntered = TempData["IsCorrectEntered"];
                ViewBag.Placeholder = HttpContext.Session.GetString("Placeholder");
                HttpContext.Session.Remove("Placeholder");

                return View(wordsUpdateRequest);
            }
        }

        [HttpPost]
        //Url: persons/create
        [Route("/word-trainer/add-user-word")]
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

                return RedirectToAction("IndexWordTrainer", "WordTrainer");
            }
            catch (Exception)
            {
                return BadRequest("Invalid inputs");
            }
        }

        [HttpPost]
        //Url: persons/create
        [Route("/word-trainer/check-word")]
        public async Task<IActionResult> EnterWord(WordsUpdateRequest wordsUpdateRequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            WordsResponse wordNextToCheck = _wordsService.GetNextWordToCheck(wordsUpdateRequest.Word, user.Id);
            TempData["IsCorrectEntered"] = false;
            try
            {
                WordsResponse? tepWord = _wordsService.GetWordById(wordsUpdateRequest.DefaultWordId,
                        wordsUpdateRequest.UserWordId, user.Id);
                bool isCorrectEntered = tepWord!.Word == wordsUpdateRequest.Word;
                

                if (isCorrectEntered)
                {
                    WordsResponse wordToCheck = _wordsService.GetWordToCheck(user.Id);
                    TempData["IsCorrectEntered"] = true;
                    wordsUpdateRequest.IsIncreaseCorrectEnteredCount = true;

                    WordsResponse updatedWord = _wordsService.UpdateWord(wordsUpdateRequest);

                    wordsUpdateRequest.DefaultWordId = wordNextToCheck.DefaultWordId;
                    wordsUpdateRequest.UserWordId = wordNextToCheck.UserWordId;
                    wordsUpdateRequest.UserId = wordNextToCheck.UserId;
                    wordsUpdateRequest.Word = wordNextToCheck.Word;
                    wordsUpdateRequest.ImageSrc = wordNextToCheck.ImageSrc;
                    wordsUpdateRequest.WordTranslation = wordNextToCheck.WordTranslation;
                    wordsUpdateRequest.CorrectEnteredCount = wordNextToCheck.CorrectEnteredCount;
                    wordsUpdateRequest.IncorrectEnteredCount = wordNextToCheck.IncorrectEnteredCount;
                    wordsUpdateRequest.LastTimeEntered = wordNextToCheck.LastTimeEntered;

                    return RedirectToAction("IndexWordTrainer", "WordTrainer", wordsUpdateRequest);

                }
                else
                {
                    TempData["IsCorrectEntered"] = false;
                    wordsUpdateRequest.IsIncreaseIncorrectEnteredCount = true;

                    WordsResponse updatedWord = _wordsService.UpdateWord(wordsUpdateRequest);
                    WordsResponse? currentWord = _wordsService.GetWordById(wordsUpdateRequest.DefaultWordId,
                        wordsUpdateRequest.UserWordId, user.Id);

                    WordsUpdateRequest? wordsNewUpdateRequest = new WordsUpdateRequest()
                    {
                        DefaultWordId = currentWord?.DefaultWordId,
                        UserWordId = currentWord?.UserWordId,
                        UserId = currentWord?.UserId,
                        Word = currentWord!.Word,
                        WordTranslation = currentWord.WordTranslation,
                        CorrectEnteredCount = currentWord.CorrectEnteredCount,
                        IncorrectEnteredCount = currentWord.IncorrectEnteredCount,
                        LastTimeEntered = currentWord.LastTimeEntered
                    };

                    return RedirectToAction("IndexWordTrainer", "WordTrainer", wordsNewUpdateRequest);
                }

            }
            catch (Exception)
            {
                return BadRequest("Invalid inputs");
            }
        }

        [HttpPost]
        [Route("/word-trainer/use-hint")]
        public async Task<IActionResult> UseHint(WordsUpdateRequest wordsUpdateRequest)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            TempData["IsCorrectEntered"] = false;
            try
            {
                WordsResponse? tepWord = _wordsService.GetWordById(wordsUpdateRequest.DefaultWordId,
                        wordsUpdateRequest.UserWordId, user.Id);

                string answer = tepWord?.Word;
                HttpContext.Session.SetString("Placeholder", answer);
                TempData["IsCorrectEntered"] = false;

                wordsUpdateRequest.IsIncreaseIncorrectEnteredCount = true;

                WordsResponse updatedWord = _wordsService.UpdateWord(wordsUpdateRequest);
                WordsResponse? currentWord = _wordsService.GetWordById(wordsUpdateRequest.DefaultWordId,
                    wordsUpdateRequest.UserWordId, user.Id);

                WordsUpdateRequest? wordsNewUpdateRequest = new WordsUpdateRequest()
                {
                    DefaultWordId = currentWord?.DefaultWordId,
                    UserWordId = currentWord?.UserWordId,
                    UserId = currentWord?.UserId,
                    Word = currentWord!.Word,
                    WordTranslation = currentWord.WordTranslation,
                    ImageSrc = currentWord.ImageSrc,
                    CorrectEnteredCount = currentWord.CorrectEnteredCount,
                    IncorrectEnteredCount = currentWord.IncorrectEnteredCount,
                    LastTimeEntered = currentWord.LastTimeEntered
                };

                return RedirectToAction("IndexWordTrainer", "WordTrainer", wordsNewUpdateRequest);
            }
            catch (Exception)
            {
                return BadRequest("Invalid inputs");
            }
        }
    }
}
