using Microsoft.AspNetCore.Mvc;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;

namespace EnjOffer.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IDefaultWordsService _defaultWordsService;
        private readonly IUsersDefaultWordsService _usersDefaultWordsService;
        private readonly IUserStatisticsService _userStatisticsService;

        public HomeController(IUsersService usersService, IDefaultWordsService defaultWordsService,
            IUsersDefaultWordsService usersDefaultWordsService, IUserStatisticsService userStatisticsService)
        {
            _usersService = usersService;
            _defaultWordsService = defaultWordsService;
            _usersDefaultWordsService = usersDefaultWordsService;
            _userStatisticsService = userStatisticsService;
        }

        [Route("/")]
        [HttpGet]
        public IActionResult IndexPersonalCabinet()
        {
            List<UserStatisticsResponse> userStatistics = _userStatisticsService.GetAllUserStatistics();

            return View("IndexPersonalCabinet", userStatistics);
        }

        [Route("/personal-cabinet/add-user")]
        [HttpPost]
        public IActionResult AddUser(UserAddRequest userAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(userAddRequest);
            }

            try
            {
                //call the service method
                UserResponse user = _usersService.AddUser(userAddRequest);

                List<UserResponse> users = _usersService.GetAllUsers();
                List<DefaultWordResponse> defaultWords = _defaultWordsService.GetAllDefaultWords();

                foreach (DefaultWordResponse itemDefaultWord in defaultWords)
                {
                    foreach (UserResponse itemUser in users)
                    {
                        UsersDefaultWordsAddRequest usersDefaultWordsAddRequest = new UsersDefaultWordsAddRequest()
                        {
                            DefaultWordId = itemDefaultWord.DefaultWordId,
                            UserId = itemUser.UserId
                        };

                        _usersDefaultWordsService.AddUserDefaultWord(usersDefaultWordsAddRequest);
                    }
                }

                return RedirectToAction("IndexPersonalCabinet", "Home");
            }
            catch (Exception)
            {
                return BadRequest("Invalid inputs");
            }
        }

        [Route("/personal-cabinet/delete-user")]
        [HttpPost]
        public IActionResult DeleteUser(string Email)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

            try
            {
                //call the service method
                bool isDeleted = _usersService.DeleteUserByEmail(Email);

                return RedirectToAction("IndexPersonalCabinet", "Home");
            }
            catch (Exception)
            {
                return BadRequest("Invalid inputs");
            }
        }

        [Route("/personal-cabinet/add-default-word")]
        [HttpPost]
        public IActionResult AddDefaultWord(DefaultWordAddRequest defaultWordAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(defaultWordAddRequest);
            }

            try
            {
                //call the service method
                DefaultWordResponse defaultWord = _defaultWordsService.AddDefaultWord(defaultWordAddRequest);

                List<UserResponse> users = _usersService.GetAllUsers();
                List<DefaultWordResponse> defaultWords = _defaultWordsService.GetAllDefaultWords();

                foreach (UserResponse user in users)
                {
                    foreach (DefaultWordResponse itemDefaultWord in defaultWords)
                    {
                        UsersDefaultWordsAddRequest usersDefaultWordsAddRequest = new UsersDefaultWordsAddRequest()
                        {
                            DefaultWordId = itemDefaultWord.DefaultWordId,
                            UserId = user.UserId
                        };

                        _usersDefaultWordsService.AddUserDefaultWord(usersDefaultWordsAddRequest);
                    }
                }

                return RedirectToAction("IndexPersonalCabinet", "Home");
            }
            catch (Exception)
            {
                return BadRequest("Invalid inputs");
            }
        }

        [Route("/personal-cabinet/delete-default-word")]
        [HttpPost]
        public IActionResult DeleteDefaultWord(string Word, string WordTranslation)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

            try
            {
                //call the service method
                bool isDeleted = _defaultWordsService.DeleteDefaultWordByWordAndTranslation(Word, WordTranslation);

                return RedirectToAction("IndexPersonalCabinet", "Home");
            }
            catch (Exception)
            {
                return BadRequest("Invalid inputs");
            }
        }
    }
}
