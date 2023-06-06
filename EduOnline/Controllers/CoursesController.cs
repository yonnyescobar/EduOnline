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
                .Include(c => c.CourseImages)
                .Include(c => c.CourseCategories)
                .ThenInclude(cc => cc.Category)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AddCourseViewModel addCourseViewModel = new()
            {
                Categories = await _dropDownListsHelper.GetDDLCategoriesAsync(),                
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

                    Course course = new()
                    {
                        Name = addCourseViewModel.Name,
                        Description = addCourseViewModel.Description,
                        Requirements = addCourseViewModel.Requirements,
                        Duration = addCourseViewModel.Duration,
                        Price = addCourseViewModel.Price,
                        CreatedDate = DateTime.Now
                    };

                    course.CourseCategories = new List<CourseCategory>()
                    {
                        new CourseCategory
                        {
                            Category = await _context.Categories.FindAsync(addCourseViewModel.CategoryId)
                        }
                    };

                    if (imageId != Guid.Empty)
                    {
                        course.CourseImages = new List<CourseImage>()
                        {
                            new CourseImage { ImageId = imageId }
                        };
                    }

                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un curso con el mismo nombre.");
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
            return View(addCourseViewModel);
        }

        public async Task<IActionResult> Edit(Guid? courseId)
        {
            if (courseId == null) return NotFound();

            Course course = await _context.Courses.FindAsync(courseId);
            if (course == null) return NotFound();

            EditCourseViewModel editCourseViewModel = new()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Requirements = course.Requirements,
                Duration = course.Duration,
                Price = course.Price,
            };

            return View(editCourseViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? Id, EditCourseViewModel editCourseViewModel)
        {
            if (Id != editCourseViewModel.Id) return NotFound();

            try
            {
                Course course = await _context.Courses.FindAsync(editCourseViewModel.Id);
                course.Name = editCourseViewModel.Name;
                course.Description = editCourseViewModel.Description;
                course.Requirements = editCourseViewModel.Requirements;
                course.Duration = editCourseViewModel.Duration;
                course.Price = editCourseViewModel.Price;
                course.Description = editCourseViewModel.Description;

                _context.Update(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    ModelState.AddModelError(string.Empty, "Ya existe un Curso con el mismo nombre.");
                else
                    ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View(editCourseViewModel);
        }

        public async Task<IActionResult> Details(Guid? courseId)
        {
            if (courseId == null) return NotFound();

            Course course = await _context.Courses
                .Include(c => c.CourseImages)                
                .Include(c => c.CourseCategories)
                .ThenInclude(cc => cc.Category)
                .FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null) return NotFound();

            return View(course);
        }

        #endregion
    }
}
