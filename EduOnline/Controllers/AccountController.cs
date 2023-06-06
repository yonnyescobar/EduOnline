using EduOnline.DAL;
using EduOnline.Helpers;
using EduOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduOnline.Controllers
{
    public class AccountController : Controller
    {
        #region Dependencies & Properties
        private readonly IUserHelper _userHelper;
        private readonly DatabaseContext _context;
        private readonly IDropDownListsHelper _ddlHelper;
        private readonly IAzureBlobHelper _azureBlobHelper;

        public AccountController(IUserHelper userHelper, DatabaseContext context, IDropDownListsHelper dropDownListsHelper, IAzureBlobHelper azureBlobHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _ddlHelper = dropDownListsHelper;
            _azureBlobHelper = azureBlobHelper;
        }

        #endregion

        #region Methods & Actions
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(loginViewModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Unauthorized()
        {
            return View();
        }

        #endregion
    }
}
