using EduOnline.Enums;
using System.ComponentModel.DataAnnotations;

namespace EduOnline.DAL.Entities
{
    public class Order : Entity
    {
        public User User { get; set; }
                
        [Display(Name = "Estado Orden")]
        public OrderStatus OrderStatus { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Líneas")]
        public int Lines => OrderDetails == null ? 0 : OrderDetails.Count;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Quantity => OrderDetails == null ? 0 : OrderDetails.Sum(sd => sd.Quantity);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => OrderDetails == null ? 0 : OrderDetails.Sum(sd => sd.Value);
    }
}
