using System.ComponentModel.DataAnnotations;

namespace EduOnline.DAL.Entities
{
    public class Country : Entity
    {
        [Display(Name = "País")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe ser de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

    }
}
