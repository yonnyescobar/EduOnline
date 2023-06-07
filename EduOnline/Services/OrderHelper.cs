using EduOnline.DAL.Entities;
using EduOnline.DAL;
using EduOnline.Enums;
using EduOnline.Helpers;
using EduOnline.Models;
using EduOnline.Common;

namespace EduOnline.Services
{
    public class OrderHelper : IOrderHelper
    {
        #region Dependencies & Properties
        private readonly DatabaseContext _context;

        public OrderHelper(DatabaseContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<Response> ProcessOrderAsync(ShowCartViewModel showCartViewModel)
        {
            Response response = await CheckQuantityAsync(showCartViewModel);
            if (!response.IsSuccess) return response;

            Order order = new()
            {
                CreatedDate = DateTime.Now,
                User = showCartViewModel.User,
                OrderDetails = new List<OrderDetail>(),
                OrderStatus = OrderStatus.Nuevo
            };

            foreach (TemporalOrder? item in showCartViewModel.TemporalOrders)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    Course = item.Course,
                    Quantity = item.Quantity,
                });

                Course course = await _context.Courses.FindAsync(item.Course.Id);
                if (course != null)
                {
                    _context.Courses.Update(course);
                }

                _context.TemporalOrders.Remove(item);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return response;
        }

        private async Task<Response> CheckQuantityAsync(ShowCartViewModel showCartViewModel)
        {
            Response response = new()
            {
                IsSuccess = true
            };

            foreach (TemporalOrder item in showCartViewModel.TemporalOrders)
            {
                Course course = await _context.Courses.FindAsync(item.Course.Id);

                if (course == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"El Curso {item.Course.Name}, ya no está disponible";
                    return response;
                }

                if (1 < item.Quantity)
                {
                    response.IsSuccess = false;
                    response.Message = $"Lo sentimos, la cantidad es {item.Quantity} para el curso {item.Course.Name}";
                    return response;
                }
            }
            return response;
        }

        #endregion
    }
}
