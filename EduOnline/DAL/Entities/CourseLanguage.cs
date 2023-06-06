namespace EduOnline.DAL.Entities
{
    public class CourseLanguage : Entity
    {
        public Course Course { get; set; }

        public Language Language { get; set; }
    }
}
