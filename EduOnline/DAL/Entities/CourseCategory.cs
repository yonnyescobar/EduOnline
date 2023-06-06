namespace EduOnline.DAL.Entities
{
    public class CourseCategory : Entity
    {
        public Course Course { get; set; }

        public Category Category { get; set; }
    }
}
