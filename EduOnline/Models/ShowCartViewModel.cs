using EduOnline.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace EduOnline.Models
{
    public class ShowCartViewModel
    {
        public User User { get; set; }

        public ICollection<TemporalOrder> TemporalOrders { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Quantity => TemporalOrders == null ? 0 : TemporalOrders.Sum(to => to.Quantity);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => TemporalOrders == null ? 0 : TemporalOrders.Sum(to => to.Value);
    }
}
