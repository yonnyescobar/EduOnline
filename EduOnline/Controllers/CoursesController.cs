using EduOnline.DAL;
using EduOnline.DAL.Entities;
using EduOnline.Helpers;
using EduOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EduOnline.Controllers
{
    [Authorize(Roles = "Administrador")]
    [AllowAnonymous]
    public class CoursesController : Controller
    {
        #region Dependencies & Properties
        private readonly DatabaseContext _context;
        private readonly IDropDownListsHelper _dropDownListsHelper;
        private readonly IAzureBlobHelper _azureBlobHelper;

        public CoursesController(DatabaseContext context, IDropDownListsHelper dropDownListsHelper, IAzureBlobHelper azureBlobHelper)
        {
            _context = context;
            _dropDownListsHelper = dropDownListsHelper;
            _azureBlobHelper = azureBlobHelper;
        }

        #endregion

        #region Methods & Actions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.CourseLanguages)
                .ThenInclude(cl => cl.Language)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AddCourseViewModel addCourseViewModel = new()
            {
                Categories = await _dropDownListsHelper.GetDDLCategoriesAsync(),
                Languages = await _dropDownListsHelper.GetDDLLanguagesAsync(),
            };

            return View(addCourseViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCourseViewModel addCourseViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (addCourseViewModel.ImageFile != null)
                        imageId = await _azureBlobHelper.UploadAzureBlobAsync(addCourseViewModel.ImageFile, "products");

                    addCourseViewModel.ImageId = imageId;

                    Course course = new()
                    {
                        Name = addCourseViewModel.Name,
                        Description = addCourseViewModel.Description,
                        Requirements = addCourseViewModel.Requirements,
                        Duration = addCourseViewModel.Duration,
                        Price = addCourseViewModel.Price,
                        CreatedDate = DateTime.Now
                    };

                    //Estoy capturando la categoría del prod para luego guardar esa relación en la tabla ProductCategories
                    course.CourseLanguages = new List<CourseLanguage>()
                    {
                        new CourseLanguage
                        {
                            Language = await _context.Languages.FindAsync(addCourseViewModel.LanguageId)
                        }
                    };

                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un producto con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            addCourseViewModel.Categories = await _dropDownListsHelper.GetDDLCategoriesAsync();
            addCourseViewModel.Languages = await _dropDownListsHelper.GetDDLLanguagesAsync();
            return View(addCourseViewModel);
        }

        #endregion
    }
}
