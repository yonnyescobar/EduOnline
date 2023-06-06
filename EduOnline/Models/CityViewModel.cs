using EduOnline.DAL.Entities;

namespace EduOnline.Models
{
    public class CityViewModel : City
    {
        public Guid StateId { get; set; }
    }
}
