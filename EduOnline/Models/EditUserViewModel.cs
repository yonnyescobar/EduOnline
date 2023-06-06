using EduOnline.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EduOnline.Models
{
    public class EditUserViewModel : Entity
    {
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo {0} solo permite datos numéricos.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Foto")]
        public Guid PhotoId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => PhotoId == Guid.Empty
            ? $"https://localhost:7217/images/NoPhoto.png"
            : $"https://sales2023.blob.core.windows.net/users/{PhotoId}";

        [Display(Name = "Foto")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "País")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Display(Name = "Departamento/Estado")]        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid StateId { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name = "Ciudad")]
        public Guid CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }
    }
}
