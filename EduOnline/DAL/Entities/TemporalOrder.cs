﻿using System.ComponentModel.DataAnnotations;

namespace EduOnline.DAL.Entities
{
    public class TemporalOrder : Entity
    {
        public ICollection<OrderDetail> OrderDetails { get; set; }

        public User User { get; set; }

        public Course Course { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => Course == null ? 0 : (decimal)Quantity * Course.Price;
    }
}
