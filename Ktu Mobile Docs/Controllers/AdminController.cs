using KTU_Mobile_Data;
using Ktu_Mobile_Docs.Models;
using Ktu_Mobile_Docs.Models.AccountViewModels;
using KTU_Mobile_Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ktu_Mobile_Docs.Controllers
{
    public class AdminController : Controller
    {
        private readonly IData _Service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Data _context;

        public AdminController(
            Data context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IData Service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _Service = Service;
        }
        //
        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new IndexAdmin
                {
                    //UserID = await _Service.GetMember(User.Identity.Name),
                    UserID = null,
                    latestinfo = await _context.Log.ToListAsync(),
                    members = await _context.Members.Include(log => log.Log).ToListAsync()
                };
                return View(model);
            }
            catch
            {
                return View();
            }
        }

        #region Category

        [Authorize]
        public async Task<IActionResult> CategoryIndex()
        {
            return View(await _context.Categorys.ToListAsync());
        }

        [Authorize]
        public IActionResult CategoryCreate()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryCreate([Bind("Id,Name,Nr,Text,Created")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                //await _Service.addlog("Pridėjo nauja skiltį \""+ category.Name +"\" dokumentacijose. Nr - " + category.Nr, null, User.Identity.Name, null);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CategoryIndex));
            }
            return View(category);
        }

        public async Task<IActionResult> CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categorys.SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryEdit(int id, [Bind("Id,Name,Nr,Text,Created")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    //await _Service.addlog("Atnaujino skiltį \"" + category.Name + "\" dokumentacijose. Nr - " + category.Nr, null, User.Identity.Name, null);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CategoryIndex));
            }
            return View(category);
        }

        [Authorize]
        public async Task<IActionResult> CategoryDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categorys
                .SingleOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [Authorize]
        [HttpPost, ActionName("CategoryDeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryDeleteConfirmed(int id)
        {
            var category = await _context.Categorys.SingleOrDefaultAsync(m => m.Id == id);
            ///await _Service.addlog("Ištrynė skiltį \"" + category.Name + "\" dokumentacijose. Nr - " + category.Nr, null, User.Identity.Name, null);
            _context.Categorys.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CategoryIndex));
        }

        [Authorize]
        private bool CategoryExists(int id)
        {
            return _context.Categorys.Any(e => e.Id == id);
        }

        #endregion Category

        #region Uers

        // GET: Users
        [Authorize]
        public async Task<IActionResult> UsersIndex()
        {
            return View(await _context.Members.ToListAsync());
        }

        public async Task<IActionResult> UsersEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Members.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsersEdit(int id, [Bind("Id,Name,Pareigos,Fb,Git,Gmail,Instragram,Image")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    //await _Service.addlog("Atnaujino asmeninę informaciją \"" + user.Name, null, User.Identity.Name, null);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UsersIndex));
            }
            return View(user);
        }

        [Authorize]
        private bool UserExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }

        #endregion Uers

        #region Loggger

        [Authorize]
        public async Task<IActionResult> LogIndex()
        {
            return View(await _context.Log.ToListAsync());
        }

        // GET: logs/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log
                .SingleOrDefaultAsync(m => m.Id == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // GET: logs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Created,Description")] log log)
        {
            if (ModelState.IsValid)
            {
                _context.Add(log);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(log);
        }

        // GET: logs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log.SingleOrDefaultAsync(m => m.Id == id);
            if (log == null)
            {
                return NotFound();
            }
            return View(log);
        }

        // POST: logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Created,Description")] log log)
        {
            if (id != log.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(log);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!logExists(log.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(log);
        }

        // GET: logs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _context.Log
                .SingleOrDefaultAsync(m => m.Id == id);
            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // POST: logs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var log = await _context.Log.SingleOrDefaultAsync(m => m.Id == id);
            _context.Log.Remove(log);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool logExists(int id)
        {
            return _context.Log.Any(e => e.Id == id);
        }

        #endregion Loggger

        #region MainPage

        [Authorize]
        public async Task<IActionResult> Info()
        {
            var main_page = await _context.Main_page.SingleOrDefaultAsync(m => m.Id == 1);
            if (main_page == null)
            {
                return NotFound();
            }

            return View(main_page);
        }

        [Authorize]
        public async Task<IActionResult> Update()
        {
            var main_page = await _context.Main_page.SingleOrDefaultAsync(m => m.Id == 1);
            if (main_page == null)
            {
                return NotFound();
            }
            return View(main_page);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("Id,Git_hub_link,Gmail,Google_store_app")] Main_page main_page)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(main_page);
                    //await _Service.addlog("Atnaujino pagrindinę puslapio informaciją", null, User.Identity.Name, null);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Main_pageExists(1))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(main_page);
        }

        private bool Main_pageExists(int id)
        {
            return _context.Main_page.Any(e => e.Id == id);
        }

        #endregion MainPage

        #region Login
        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false);
                //await _Service.addlog("User logged in ", null, model.Email, null);
                await _context.SaveChangesAsync();
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                //_logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                //_logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                //_logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    _logger.LogInformation("User logged out.");
        //    return RedirectToAction(nameof(HomeController.Index), "Gallery");
        //}

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Index");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                //string[] arr = { "vaitkus.pvls@gmail.com", "lukas.liudzius@gmail.com", "renaldas.stilpa@gmail.com", "modelysmo@gmail.com", "lukjok7@gmail.com", "zilvinasnorinkevicius78@gmail.com" };
                //int pos = Array.FindIndex(arr, x => x.ToLower() == model.Email.ToLower());
                //if (pos > -1)
                //{
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User created a new account with password.");
                    //await _Service.addlog("User created a new account with password.", null, model.Email, null);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //_logger.LogInformation("User created a new account with password.");
                    //await _Service.addlog("User created a new account with password.", null, model.Email, null);
                   await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Admin");
                }
                return RedirectToAction("Login", "Admin");
                //AddErrors(result);
                //}
                //return RedirectToAction("Index", "Admin");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                //await _Service.addlog("Unable to load user with ID .", Int32.Parse(userId), null, null);
                await _context.SaveChangesAsync();
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        #endregion Helpers
        #endregion
    }
}