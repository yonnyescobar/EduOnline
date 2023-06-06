﻿using EduOnline.DAL;
using EduOnline.DAL.Entities;
using EduOnline.Enums;
using EduOnline.Helpers;
using EduOnline.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            Guid emptyGuid = new Guid();

            AddUserViewModel addUserViewModel = new()
            {
                Id = Guid.Empty,
                Countries = await _ddlHelper.GetDDLCountriesAsync(),
                States = await _ddlHelper.GetDDLStatesAsync(emptyGuid),
                Cities = await _ddlHelper.GetDDLCitiesAsync(emptyGuid),
                UserType = UserType.Estudiante,
            };

            return View(addUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel addUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (addUserViewModel.ImageFile != null)
                    imageId = await _azureBlobHelper.UploadAzureBlobAsync(addUserViewModel.ImageFile, "users");

                addUserViewModel.PhotoId = imageId;
                addUserViewModel.CreatedDate = DateTime.Now;

                User user = await _userHelper.AddUserAsync(addUserViewModel);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    await FillDropDownListLocation(addUserViewModel);
                    return View(addUserViewModel);
                }

                LoginViewModel loginViewModel = new()
                {
                    Password = addUserViewModel.Password,
                    RememberMe = false,
                    Username = addUserViewModel.Username
                };

                var login = await _userHelper.LoginAsync(loginViewModel);

                if (login.Succeeded) return RedirectToAction("Index", "Home");
            }

            await FillDropDownListLocation(addUserViewModel);
            return View(addUserViewModel);
        }

        private async Task FillDropDownListLocation(AddUserViewModel addUserViewModel)
        {
            addUserViewModel.Countries = await _ddlHelper.GetDDLCountriesAsync();
            addUserViewModel.States = await _ddlHelper.GetDDLStatesAsync(addUserViewModel.CountryId);
            addUserViewModel.Cities = await _ddlHelper.GetDDLCitiesAsync(addUserViewModel.StateId);
        }

        public async Task<IActionResult> EditUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null) return NotFound();

            EditUserViewModel editUserViewModel = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PhotoId = user.PhotoId,
                Cities = await _ddlHelper.GetDDLCitiesAsync(user.City.State.Id),
                CityId = user.City.Id,
                Countries = await _ddlHelper.GetDDLCountriesAsync(),
                CountryId = user.City.State.Country.Id,
                StateId = user.City.State.Id,
                States = await _ddlHelper.GetDDLStatesAsync(user.City.State.Country.Id),
                Id = Guid.Parse(user.Id),
                Document = user.Document
            };

            return View(editUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = editUserViewModel.PhotoId;

                if (editUserViewModel.ImageFile != null)
                    imageId = await _azureBlobHelper.UploadAzureBlobAsync(editUserViewModel.ImageFile, "users");

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.FirstName = editUserViewModel.FirstName;
                user.LastName = editUserViewModel.LastName;
                user.PhoneNumber = editUserViewModel.PhoneNumber;
                user.PhotoId = imageId;
                user.City = await _context.Cities.FindAsync(editUserViewModel.CityId);
                user.Document = editUserViewModel.Document;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            editUserViewModel.Countries = await _ddlHelper.GetDDLCountriesAsync();
            editUserViewModel.States = await _ddlHelper.GetDDLStatesAsync(editUserViewModel.CountryId);
            editUserViewModel.Cities = await _ddlHelper.GetDDLCitiesAsync(editUserViewModel.StateId);

            return View(editUserViewModel);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                if (changePasswordViewModel.CurrentPassword == changePasswordViewModel.NewPassword)
                {
                    ModelState.AddModelError(string.Empty, "Debes ingresar una contraseña diferente.");
                    return View(changePasswordViewModel);
                }

                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                if (user != null)
                {
                    IdentityResult result = await _userHelper.ChangePasswordAsync(user, changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);
                    if (result.Succeeded) return RedirectToAction("EditUser");
                    else ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                }
                else ModelState.AddModelError(string.Empty, "Usuario no encontrado");
            }

            return View(changePasswordViewModel);
        }

        [HttpGet]
        public JsonResult GetStates(Guid countryId)
        {
            Country country = _context.Countries
                .Include(c => c.States)
                .FirstOrDefault(c => c.Id == countryId);

            if (country == null) return null;

            return Json(country.States.OrderBy(d => d.Name));
        }

        [HttpGet]
        public JsonResult GetCities(Guid stateId)
        {
            State state = _context.States
                .Include(s => s.Cities)
                .FirstOrDefault(s => s.Id == stateId);
            if (state == null) return null;

            return Json(state.Cities.OrderBy(c => c.Name));
        }

        #endregion
    }
}
