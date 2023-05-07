using EnjOffer.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using EnjOffer.UI.ViewModels;

namespace EnjOffer.UI.Controllers
{
    public class WordTrainerController : Controller
    {
        private readonly IDefaultWordsService _defaultWordsService;
        private readonly IUsersDefaultWordsService _usersDefaultWordsService;
        private readonly IUserWordsService _userWordsService;
        private readonly IWordsService _wordsService;

        public WordTrainerController(IDefaultWordsService defaultWordsService, 
            IUsersDefaultWordsService usersDefaultWordsService, IUserWordsService userWordsService,
            IWordsService wordsService)
        {
            _defaultWordsService = defaultWordsService;
            _usersDefaultWordsService = usersDefaultWordsService;
            _userWordsService = userWordsService;
            _wordsService = wordsService;
        }

        [Route("word-trainer")]
        [HttpGet]
        public IActionResult IndexWordTrainer(WordsUpdateRequest? wordsUpdateRequest)
        {
            WordsResponse wordToCheck = _wordsService.GetWordToCheck();

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
                    LastTimeEntered = wordToCheck.LastTimeEntered
                };

                ViewBag.IsCorrectEntered = TempData["IsCorrectEntered"];

                return View(wordsNewUpdateRequest);
            }
            else
            {
                ViewBag.IsCorrectEntered = TempData["IsCorrectEntered"];
                //wordTrainerViewModel.wordsResponse = wordToCheck;
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
        public IActionResult EnterWord(WordsUpdateRequest wordsUpdateRequest)
        {
            WordsResponse wordToCheck = _wordsService.GetWordToCheck();
            WordsResponse wordNextToCheck = _wordsService.GetNextWordToCheck();
            TempData["IsCorrectEntered"] = false;
            try
            {
                bool isCorrectEntered = _wordsService.CheckWord(wordsUpdateRequest.Word!);
                
                if (isCorrectEntered)
                {
                    TempData["IsCorrectEntered"] = true;
                    wordsUpdateRequest.IsIncreaseCorrectEnteredCount = true;

                    WordsResponse updatedWord = _wordsService.UpdateWord(wordsUpdateRequest);

                    wordsUpdateRequest.DefaultWordId = wordNextToCheck.DefaultWordId;
                    wordsUpdateRequest.UserWordId = wordNextToCheck.UserWordId;
                    wordsUpdateRequest.UserId = wordNextToCheck.UserId;
                    wordsUpdateRequest.Word = wordNextToCheck.Word;
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

                    WordsUpdateRequest? wordsNewUpdateRequest = new WordsUpdateRequest()
                    {
                        DefaultWordId = wordToCheck.DefaultWordId,
                        UserWordId = wordToCheck.UserWordId,
                        UserId = wordToCheck.UserId,
                        Word = wordToCheck.Word,
                        WordTranslation = wordToCheck.WordTranslation,
                        CorrectEnteredCount = wordToCheck.CorrectEnteredCount,
                        IncorrectEnteredCount = wordToCheck.IncorrectEnteredCount,
                        LastTimeEntered = wordToCheck.LastTimeEntered
                    };

                    return RedirectToAction("IndexWordTrainer", "WordTrainer", wordsNewUpdateRequest);
                }

            }
            catch (Exception)
            {
                return BadRequest("Invalid inputs");
            }
        }
    }
}
