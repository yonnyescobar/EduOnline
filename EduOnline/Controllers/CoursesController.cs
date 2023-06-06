using EduOnline.DAL;
using EduOnline.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduOnline.Controllers
{
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

        #endregion
    }
}
