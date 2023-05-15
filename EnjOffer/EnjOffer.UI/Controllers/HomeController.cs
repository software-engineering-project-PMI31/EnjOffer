using Microsoft.AspNetCore.Mvc;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;
using Microsoft.AspNetCore.Identity;
using EnjOffer.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EnjOffer.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDefaultWordsService _defaultWordsService;
        private readonly IUsersDefaultWordsService _usersDefaultWordsService;
        private readonly IUserStatisticsService _userStatisticsService;
        private readonly IUsersService _usersService;
        private readonly IAdviceService _adviceService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public HomeController(IUsersService usersService, IDefaultWordsService defaultWordsService,
            IUsersDefaultWordsService usersDefaultWordsService, IUserStatisticsService userStatisticsService,
            IAdviceService adviceService, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _usersService = usersService;
            _defaultWordsService = defaultWordsService;
            _usersDefaultWordsService = usersDefaultWordsService;
            _userStatisticsService = userStatisticsService;
            _adviceService = adviceService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> IndexPersonalCabinet()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            List<UserStatisticsResponse> userStatistics = _userStatisticsService.GetAllUserStatistics(user.Id);

            string? serializedErrorsDeleteAdminList = HttpContext.Session.GetString("ErrorsDeleteAdmin");
            List<string>? errorsDeleteAdminList = serializedErrorsDeleteAdminList is not null ? 
                JsonConvert.DeserializeObject<List<string>>(serializedErrorsDeleteAdminList) : new List<string>();

            string? serializedErrorsAddDefaultWordList = HttpContext.Session.GetString("ErrorsAddDefaultWord");
            List<string>? errorsAddDefaultWordList = serializedErrorsAddDefaultWordList is not null ?
                JsonConvert.DeserializeObject<List<string>>(serializedErrorsAddDefaultWordList) : new List<string>();

            string? serializedErrorsRegisterList = HttpContext.Session.GetString("ErrorsRegister");
            List<string>? errorsRegisterList = serializedErrorsRegisterList is not null ?
                JsonConvert.DeserializeObject<List<string>>(serializedErrorsRegisterList) : new List<string>();

            string? serializedErrorsDeleteDefaultWordList = HttpContext.Session.GetString("ErrorsDeleteDefaultWord");
            List<string>? errorsDeleteDefaultWordList = serializedErrorsDeleteDefaultWordList is not null ?
                JsonConvert.DeserializeObject<List<string>>(serializedErrorsDeleteDefaultWordList) : new List<string>();

            ViewBag.ErrorsRegister = errorsRegisterList;
            ViewBag.ErrorsDeleteAdmin = errorsDeleteAdminList;
            ViewBag.ErrorsAddDefaultWord = errorsAddDefaultWordList;
            ViewBag.ErrorsDeleteDefaultWord = errorsDeleteDefaultWordList;

            HttpContext.Session.Remove("ErrorsDeleteAdmin");
            HttpContext.Session.Remove("ErrorsAddDefaultWord");
            HttpContext.Session.Remove("ErrorsRegister");
            HttpContext.Session.Remove("ErrorsDeleteDefaultWord");

            HttpContext.Session.Remove("Book");

            ViewBag.Advice = _adviceService.GetAllAdvice();

            return View("IndexPersonalCabinet", userStatistics);
        }

        [Route("/personal-cabinet/add-admin")]
        [HttpPost]
        public async Task<IActionResult> AddAdmin(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorsRegister = ModelState.Values.SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                List<string> errors = new List<string>();
                errors.AddRange(ViewBag.ErrorsRegister);
                string serializedErrorsRegisterList = JsonConvert.SerializeObject(errors);
                HttpContext.Session.SetString("ErrorsRegister", serializedErrorsRegisterList);

                return RedirectToAction(nameof(HomeController.IndexPersonalCabinet), "Home");
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                //Check options for Role
                if (registerDTO.UserType == Core.Enums.UserTypeOptions.Admin)
                {
                    if (await _roleManager.FindByNameAsync(Core.Enums.UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = Core.Enums.UserTypeOptions.Admin.ToString()
                        };

                        await _roleManager.CreateAsync(applicationRole);
                    }

                    await _userManager.AddToRoleAsync(user, Core.Enums.UserTypeOptions.Admin.ToString());
                }

                List<ApplicationUser> users = await _usersService.GetAllUsers();
                List<DefaultWordResponse> defaultWords = _defaultWordsService.GetAllDefaultWords();

                foreach (DefaultWordResponse itemDefaultWord in defaultWords)
                {
                    foreach (ApplicationUser itemUser in users)
                    {
                        UsersDefaultWordsAddRequest usersDefaultWordsAddRequest = new UsersDefaultWordsAddRequest()
                        {
                            DefaultWordId = itemDefaultWord.DefaultWordId,
                            UserId = itemUser.Id
                        };

                        _usersDefaultWordsService.AddUserDefaultWord(usersDefaultWordsAddRequest);
                    }
                }

                return RedirectToAction(nameof(HomeController.IndexPersonalCabinet), "Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("IndexPersonalCabinet", error.Description);
                }

                ViewBag.ErrorsRegister = ModelState.Values.SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                List<string> errors = new List<string>();
                errors.AddRange(ViewBag.ErrorsRegister);
                string serializedErrorsRegisterList = JsonConvert.SerializeObject(errors);
                HttpContext.Session.SetString("ErrorsRegister", serializedErrorsRegisterList);

                return RedirectToAction(nameof(HomeController.IndexPersonalCabinet), "Home");
            }
        }

        [Route("/personal-cabinet/add-default-word")]
        [HttpPost]
        public async Task<IActionResult> AddDefaultWord(DefaultWordAddRequest defaultWordAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorsAddDefaultWord = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                
                List<string> errors = new List<string>();
                errors.AddRange(ViewBag.ViewBag.ErrorsAddDefaultWord);
                string serializedErrorsAddDefaultWordList = JsonConvert.SerializeObject(errors);
                HttpContext.Session.SetString("ErrorsAddDefaultWord", serializedErrorsAddDefaultWordList);

                return RedirectToAction(nameof(HomeController.IndexPersonalCabinet), "Home");
            }

            try
            {
                //call the service method
                DefaultWordResponse defaultWord = _defaultWordsService.AddDefaultWord(defaultWordAddRequest);

                List<ApplicationUser> users = await _usersService.GetAllUsers();
                List <DefaultWordResponse> defaultWords = _defaultWordsService.GetAllDefaultWords();

                foreach (ApplicationUser user in users)
                {
                    foreach (DefaultWordResponse itemDefaultWord in defaultWords)
                    {
                        UsersDefaultWordsAddRequest usersDefaultWordsAddRequest = new UsersDefaultWordsAddRequest()
                        {
                            DefaultWordId = itemDefaultWord.DefaultWordId,
                            UserId = user.Id
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
                ViewBag.ErrorsDeleteDefaultWord = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                List<string> errors = new List<string>();
                errors.AddRange(ViewBag.ErrorsDeleteDefaultWord);
                string serializedErrorsDeleteDefaultWordList = JsonConvert.SerializeObject(errors);
                HttpContext.Session.SetString("ErrorsDeleteDefaultWord", serializedErrorsDeleteDefaultWordList);

                return RedirectToAction("IndexPersonalCabinet", "Home");
            }

            try
            {
                //call the service method
                bool isDeleted = _defaultWordsService.DeleteDefaultWordByWordAndTranslation(Word, WordTranslation);

                return RedirectToAction("IndexPersonalCabinet", "Home");
            }
            catch (ArgumentNullException)
            {
                ViewBag.ErrorsDeleteDefaultWord = "This default word does not exist";

                List<string> errors = new List<string>();
                errors.Add(ViewBag.ErrorsDeleteDefaultWord);
                string serializedErrorsDeleteDefaultWordList = JsonConvert.SerializeObject(errors);
                HttpContext.Session.SetString("ErrorsDeleteDefaultWord", serializedErrorsDeleteDefaultWordList);

                return RedirectToAction("IndexPersonalCabinet", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorsDeleteDefaultWord = ex.Message;

                List<string> errors = new List<string>();
                errors.Add(ViewBag.ErrorsDeleteDefaultWord);
                string serializedErrorsDeleteDefaultWordList = JsonConvert.SerializeObject(errors);
                HttpContext.Session.SetString("ErrorsDeleteDefaultWord", serializedErrorsDeleteDefaultWordList);

                return RedirectToAction("IndexPersonalCabinet", "Home");
            }
        }
    }
}
