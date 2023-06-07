using EduOnline.DAL.Entities;

namespace EduOnline.Models
{
    public class CourseHomeViewModel
    {
        public ICollection<Course> Courses { get; set; }
    }
}
