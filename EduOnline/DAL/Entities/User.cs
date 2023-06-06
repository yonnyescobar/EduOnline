using EduOnline.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EduOnline.DAL.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Fecha de creación")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Ciudad")]
        public City City { get; set; }

        [Display(Name = "Foto")]
        public Guid PhotoId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => PhotoId == Guid.Empty
            ? $"https://localhost:7217/images/NoPhoto.png"
            : $"https://sales2023.blob.core.windows.net/users/{PhotoId}";

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        //Propiedades de Lectura
        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Usuario")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

    }
}

