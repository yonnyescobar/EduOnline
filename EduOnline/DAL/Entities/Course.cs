﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduOnline.DAL.Entities
{
    public class Course : Entity
    {
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        [MaxLength(1000, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Requisitos")]
        [MaxLength(1000, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        public string Requirements { get; set; }

        [Display(Name = "Duración")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Duration { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Price { get; set; }

        [Display(Name = "Idioma")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Language { get; set; }

        [Display(Name = "Categorías")]
        public ICollection<CourseCategory> CourseCategories { get; set; }
                
        [Display(Name = "Imagen")]
        public ICollection<CourseImage> CourseImages { get; set; }

        [Display(Name = "Imagen")]
        public string ImageFullPath => CourseImages == null || CourseImages.Count == 0
            ? $"https://localhost:7217/images/NoImage.png"
            : CourseImages.FirstOrDefault().ImageFullPath;
    }
}
