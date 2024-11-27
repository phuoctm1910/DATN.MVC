using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DATN.MVC.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Kiểm tra nếu đang ở trang Login, bỏ qua kiểm tra role
            var path = context.HttpContext.Request.Path.Value?.ToLower();
            if (path?.Contains("/account/login") == true)
            {
                base.OnActionExecuting(context);
                return;
            }

            // Lấy role và các thông tin khác từ HttpContext.Items
            if (!context.HttpContext.Items.ContainsKey("roleName"))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var roleName = context.HttpContext.Items["roleName"]?.ToString();
            var area = context.RouteData.Values["area"]?.ToString()?.ToLower();
            var action = context.RouteData.Values["action"]?.ToString()?.ToLower();

            // Kiểm tra quyền truy cập theo area và action
            if (area == "admin" && roleName?.ToLower() != "admin")
            {
                context.Result = new ForbidResult();
                return;
            }

            if (area == "employee" && action == "edit" && roleName?.ToLower() != "admin")
            {
                context.Result = new ForbidResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
