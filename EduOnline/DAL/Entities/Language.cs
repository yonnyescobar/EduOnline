using System.ComponentModel.DataAnnotations;

namespace EduOnline.DAL.Entities
{
    public class Language : Entity
    {
        [Display(Name = "Idioma")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe ser de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public ICollection<Language> CourseLanguages { get; set;}
    }
}
