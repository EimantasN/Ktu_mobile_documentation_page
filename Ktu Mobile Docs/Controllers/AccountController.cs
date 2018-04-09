//using KTU_Mobile_Data;
//using Ktu_Mobile_Docs.Models.AccountViewModels;
//using Ktu_Mobile_Docs.Services;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Threading.Tasks;
//using KTU_Mobile_Services;

//namespace Ktu_Mobile_Docs.Controllers
//{
//    [Route("[controller]/[action]")]
//    public class AccountController : Controller
//    {
//        private readonly IData _Service;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly IEmailSender _emailSender;
//        private readonly ILogger _logger;

//        public AccountController(
//            UserManager<ApplicationUser> userManager,
//            SignInManager<ApplicationUser> signInManager,
//            IEmailSender emailSender,
//            ILogger<AccountController> logger,
//            IData Service)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _emailSender = emailSender;
//            _logger = logger;
//            _Service = Service;
//        }

//        [TempData]
//        public string ErrorMessage { get; set; }

//        [HttpGet]
//        [AllowAnonymous]
//        public async Task<IActionResult> Login(string returnUrl = null)
//        {
//            // Clear the existing external cookie to ensure a clean login process
//            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

//            ViewData["ReturnUrl"] = returnUrl;
//            return View();
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
//        {
//            ViewData["ReturnUrl"] = returnUrl;
//            if (ModelState.IsValid)
//            {
//                // This doesn't count login failures towards account lockout
//                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
//                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
//                await _Service.addlog("User logged in ", null, model.Email, null);

//                _logger.LogInformation("User logged in.");
//                return RedirectToAction("Index", "Admin");
//            }

//            // If we got this far, something failed, redisplay form
//            return View(model);
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
//        {
//            // Ensure the user has gone through the username & password screen first
//            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

//            if (user == null)
//            {
//                throw new ApplicationException($"Unable to load two-factor authentication user.");
//            }

//            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
//            ViewData["ReturnUrl"] = returnUrl;

//            return View(model);
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }

//            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
//            if (user == null)
//            {
//                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
//            }

//            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

//            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

//            if (result.Succeeded)
//            {
//                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
//                return RedirectToLocal(returnUrl);
//            }
//            else if (result.IsLockedOut)
//            {
//                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
//                return RedirectToAction(nameof(Lockout));
//            }
//            else
//            {
//                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
//                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
//                return View();
//            }
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult Lockout()
//        {
//            return View();
//        }

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> Logout()
//        //{
//        //    await _signInManager.SignOutAsync();
//        //    _logger.LogInformation("User logged out.");
//        //    return RedirectToAction(nameof(HomeController.Index), "Gallery");
//        //}

//        [HttpGet]
//        public IActionResult AccessDenied()
//        {
//            return View();
//        }

//        #region Helpers

//        private void AddErrors(IdentityResult result)
//        {
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }

//        private IActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                return Redirect(returnUrl);
//            }
//            else
//            {
//                return RedirectToAction(nameof(HomeController.Index), "Gallery");
//            }
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult Register(string returnUrl = null)
//        {
//            ViewData["ReturnUrl"] = returnUrl;
//            return View();
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
//        {
//            ViewData["ReturnUrl"] = returnUrl;
//            if (ModelState.IsValid)
//            {
//                string[] arr = { "vaitkus.pvls@gmail.com", "lukas.liudzius@gmail.com", "renaldas.stilpa@gmail.com", "modelysmo@gmail.com", "lukjok7@gmail.com", "zilvinasnorinkevicius78@gmail.com" };
//                int pos = Array.FindIndex(arr, x => x.ToLower() == model.Email.ToLower());
//                if (pos > -1)
//                {
//                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
//                    var result = await _userManager.CreateAsync(user, model.Password);
//                    if (result.Succeeded)
//                    {
//                        _logger.LogInformation("User created a new account with password.");
//                        await _Service.addlog("User created a new account with password.", null, model.Email, null);

//                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//                        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
//                        await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

//                        await _signInManager.SignInAsync(user, isPersistent: false);
//                        _logger.LogInformation("User created a new account with password.");
//                        await _Service.addlog("User created a new account with password.", null, model.Email, null);
//                        return RedirectToLocal(returnUrl);
//                    }
//                    AddErrors(result);
//                }
//                return RedirectToAction("Index", "Admin");
//            }

//            // If we got this far, something failed, redisplay form
//            return View(model);
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        public async Task<IActionResult> ConfirmEmail(string userId, string code)
//        {
//            if (userId == null || code == null)
//            {
//                return RedirectToAction(nameof(HomeController.Index), "Home");
//            }
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null)
//            {
//                await _Service.addlog("Unable to load user with ID .", Int32.Parse(userId), null, null);
//                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
//            }
//            var result = await _userManager.ConfirmEmailAsync(user, code);
//            return View(result.Succeeded ? "ConfirmEmail" : "Error");
//        }

//        #endregion Helpers
//    }
//}