using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EduOnline.Models
{
    public class AddCourseViewModel : EditCourseViewModel
    {  
        [Display(Name = "Imagen")]
        public Guid ImageId { get; set; }

        [Display(Name = "Imagen")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7217/images/NoImage.png"
            : $"https://sales2023.blob.core.windows.net/products/{ImageId}";

        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }
    }
}
