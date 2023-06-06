﻿using System.ComponentModel.DataAnnotations;

namespace EduOnline.DAL.Entities
{
    public class State : Entity
    {
        [Display(Name = "Estado")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe ser de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es oblilgatorio.")]
        public string Name { get; set; }

        [Display(Name = "País")]
        public Country Country { get; set; }

        [Display(Name = "Ciudades")]
        public ICollection<City> Cities { get; set; }

        [Display(Name = "Número Ciudades")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count; 

    }
}
