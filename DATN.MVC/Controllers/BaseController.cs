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

            // Kiểm tra role từ HttpContext.Items
            if (!context.HttpContext.Items.ContainsKey("roleName"))
            {
                ClearContextAndRedirectToLogin(context);
                return;
            }

            var roleName = context.HttpContext.Items["roleName"]?.ToString();
            var area = context.RouteData.Values["area"]?.ToString()?.ToLower();
            var action = context.RouteData.Values["action"]?.ToString()?.ToLower();

            // Kiểm tra quyền truy cập theo area và action
            if (area == "admin" && roleName?.ToLower() != "admin")
            {
                // Chỉ admin mới được vào area admin
                ClearContextAndRedirectToLogin(context);
                return;
            }

            if (area == "employee" && roleName?.ToLower() != "admin" && roleName?.ToLower() != "employee")
            {
                // Chỉ admin và employee mới được vào area employee
                ClearContextAndRedirectToLogin(context);
                return;
            }


            base.OnActionExecuting(context);
        }

        private void ClearContextAndRedirectToLogin(ActionExecutingContext context)
        {
            // Clear tất cả session, cookies và context
            context.HttpContext.Session.Clear();

            // Xóa cookies (nếu có)
            foreach (var cookie in context.HttpContext.Request.Cookies.Keys)
            {
                context.HttpContext.Response.Cookies.Delete(cookie);
            }

            // Reset area nếu tồn tại để tránh bị thêm vào đường dẫn
            if (context.RouteData.Values.ContainsKey("area"))
            {
                context.RouteData.Values.Remove("area");
            }

            // Chuyển hướng đến trang Login
            context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
        }

    }
}
