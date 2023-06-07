using EduOnline.Common;
using EduOnline.Models;

namespace EduOnline.Helpers
{
    public interface IOrderHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel showCartViewModel);
    }
}
