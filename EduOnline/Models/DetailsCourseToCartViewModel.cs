using EduOnline.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace EduOnline.Models
{
    public class DetailsCourseToCartViewModel : Entity
    {
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(500, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Requisitos")]
        [MaxLength(1000, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        public string Requirements { get; set; }

        [Display(Name = "Duración")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Duration { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

        [Display(Name = "Idioma")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Language { get; set; }

        [Display(Name = "Categoría")]
        public string Categories { get; set; }

        public ICollection<CourseImage> CourseImages { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Range(0.0000001, float.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la cantidad.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

    }
}
