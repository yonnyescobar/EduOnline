using EduOnline.DAL.Entities;

namespace EduOnline.Models
{
    public class HomeViewModel
    {
        public ICollection<Course> Courses { get; set; }
    }
}
