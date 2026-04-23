using System.Web.Mvc;
using System.Web.Routing;

namespace WebViecLam.Filters // Đảm bảo đúng namespace của project bạn
{
    // Kế thừa từ ActionFilterAttribute để can thiệp vào quá trình trước khi load trang
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Lấy quyền (Role) của người dùng hiện tại từ Session
            var role = filterContext.HttpContext.Session["Role"] as string;

            // Kiểm tra: Nếu chưa đăng nhập HOẶC đã đăng nhập nhưng không phải Admin
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                // Báo lỗi bằng TempData để hiện popup ở trang Login
                filterContext.Controller.TempData["ErrorMsg"] = "Bạn không có quyền truy cập khu vực Quản trị!";

                // "Đá" người dùng về trang Đăng nhập
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        area = "", // Bắt buộc rỗng để thoát ra khỏi khu vực Admin
                        controller = "Account",
                        action = "Login"
                    })
                );
            }

            // Cho phép tiếp tục nếu là Admin
            base.OnActionExecuting(filterContext);
        }
    }
}