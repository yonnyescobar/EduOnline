using EduOnline.DAL;
using EduOnline.DAL.Entities;
using EduOnline.Helpers;
using EduOnline.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduOnline.Controllers
{
    public class HomeController : Controller
    {
        #region Dependencies & Properties
        private readonly DatabaseContext _context;
        private readonly IUserHelper _userHelper;

        public HomeController(DatabaseContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;            
        }

        #endregion

        #region Methods & Actions

        public async Task<IActionResult> Index()
        {
            List<Course>? courses = await _context.Courses
                .Include(c => c.CourseImages)
                .Include(c => c.CourseCategories)
                .OrderBy(p => p.Name)
                .ToListAsync();

            ViewBag.UserFullName = GetUserFullName();

            HomeViewModel homeViewModel = new() { Courses = courses };

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                //homeViewModel.Quantity = await _context.TemporalSales
                //    .Where(ts => ts.User.Id == user.Id)
                //    .SumAsync(ts => ts.Quantity);
            }

            return View(homeViewModel);
        }

        private string GetUserFullName()
        {
            return _context.Users
                 .Where(u => u.Email == User.Identity.Name)
                 .Select(u => u.FullName)
                 .FirstOrDefault();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        public async Task<IActionResult> AddCourseInKart(Guid? courseId)
        {
            if (courseId == null) return NotFound();

            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");

            Course course = await _context.Courses.FindAsync(courseId);
            User user = await _userHelper.GetUserAsync(User.Identity.Name);

            if (user == null || course == null) return NotFound();

            TemporalOrder existingTemporalOrder = await _context.TemporalOrders
                .Where(t => t.Course.Id == courseId && t.User.Id == user.Id)
                .FirstOrDefaultAsync();

            if (existingTemporalOrder != null)
            {
                existingTemporalOrder.Quantity += 1;
                existingTemporalOrder.ModifiedDate = DateTime.Now;
            }
            else
            {
                TemporalOrder temporalOrder = new()
                {
                    CreatedDate = DateTime.Now,
                    Course = course,
                    Quantity = 1,
                    User = user
                };

                _context.TemporalOrders.Add(temporalOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}