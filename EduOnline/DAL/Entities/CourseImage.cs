using System.ComponentModel.DataAnnotations;

namespace EduOnline.DAL.Entities
{
    public class CourseImage : Entity
    {
        public Course Course { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7217/images/NoImage.png"
            : $"https://sales2023.blob.core.windows.net/products/{ImageId}";
    }
}
