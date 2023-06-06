using System.ComponentModel.DataAnnotations;

namespace EduOnline.DAL.Entities
{
    public class Category : Entity
    {
        [Display(Name = "Categoría")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe ser de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public ICollection<CourseCategory> CourseCategories { get; set; }

        [Display(Name = "Cursos")]
        public int CoursesNumber => CourseCategories == null ? 0 : CourseCategories.Count;
    }
}
