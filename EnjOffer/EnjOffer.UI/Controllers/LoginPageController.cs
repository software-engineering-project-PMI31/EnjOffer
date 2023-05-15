using Microsoft.AspNetCore.Mvc;
using EnjOffer.Core.DTO;
using Microsoft.AspNetCore.Identity;
using EnjOffer.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using EnjOffer.UI.EmailConfiguration;
using EnjOffer.Core.ServiceContracts;
using Newtonsoft.Json;

namespace EnjOffer.UI.Controllers
{
    [AllowAnonymous]
    public class LoginPageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IUsersService _usersService;
        private readonly IDefaultWordsService _defaultWordsService;
        private readonly IUsersDefaultWordsService _usersDefaultWordsService;

        public LoginPageController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager,
            IConfiguration config, IUsersService usersService, IDefaultWordsService defaultWordsService,
            IUsersDefaultWordsService usersDefaultWordsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _usersService = usersService;
            _defaultWordsService = defaultWordsService;
            _usersDefaultWordsService = usersDefaultWordsService;
        }

        [Route("/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorsRegister = ModelState.Values.SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                List<string> errors = new List<string>();
                errors.AddRange(ViewBag.ErrorsRegister);
                string serializedErrorsRegisterList = JsonConvert.SerializeObject(errors);
                HttpContext.Session.SetString("ErrorsRegister", serializedErrorsRegisterList);

                return View("Register", registerDTO);
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
                else
                {
                    if (await _roleManager.FindByNameAsync(Core.Enums.UserTypeOptions.User.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = Core.Enums.UserTypeOptions.User.ToString()
                        };

                        await _roleManager.CreateAsync(applicationRole);
                    }

                    await _userManager.AddToRoleAsync(user, Core.Enums.UserTypeOptions.User.ToString());
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

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains(Core.Enums.UserTypeOptions.User.ToString()))
                {
                    //Sign in
                    await _signInManager.SignInAsync(user, false);
                }

                return RedirectToAction(nameof(HomeController.IndexPersonalCabinet), "Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }
        }

        [Route("/personal-cabinet/delete-admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.IndexPersonalCabinet), "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("IndexPersonalCabinet", error.Description);
                    }

                    ViewBag.ErrorsDeleteAdmin = ModelState.Values.SelectMany(temp => temp.Errors)
                        .Select(temp => temp.ErrorMessage);

                    List<string> errors = new List<string>();
                    errors.AddRange(ViewBag.ErrorsDeleteAdmin);
                    string serializedErrorsList = JsonConvert.SerializeObject(errors);
                    HttpContext.Session.SetString("Errors", serializedErrorsList);

                    return RedirectToAction(nameof(HomeController.IndexPersonalCabinet), "Home");
                }
            }
            else
            {
                ModelState.AddModelError("IndexPersonalCabinet", "User not found");

                ViewBag.ErrorsDeleteAdmin = ModelState.Values.SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                List<string> errors = new List<string>();
                errors.AddRange(ViewBag.ErrorsDeleteAdmin);
                string serializedErrorsDeleteAdminList = JsonConvert.SerializeObject(errors);
                HttpContext.Session.SetString("ErrorsDeleteAdmin", serializedErrorsDeleteAdminList);

                return RedirectToAction(nameof(HomeController.IndexPersonalCabinet), "Home");
            }
        }

        [Route("/login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View("Login", loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password,
                isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                return RedirectToAction(nameof(HomeController.IndexPersonalCabinet), "Home");
            }

            ModelState.AddModelError("Login", "Invalid email or password");

            ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

            return View("Login", loginDTO);
        }

        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Register");
        }

        [Route("forgot-password")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("forgot-password")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "LoginPage", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService(_config);

                await emailService.SendEmailAsync(model.Email, "Reset Password",
                    $"To reset the password follow the link: <a href='{callbackUrl}'>link</a>");

                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [Route("reset-password")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string? code = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);
            }

            return View("ResetPassword");
        }

        [Route("reset-password")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
    }
}