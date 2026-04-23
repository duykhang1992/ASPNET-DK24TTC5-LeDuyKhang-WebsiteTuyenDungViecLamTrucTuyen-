using System.Linq;
using System.Web.Mvc;
using WebViecLam.Models;
using WebViecLam.Filters; // Thư viện chứa ổ khóa AdminAuthorize

namespace WebViecLam.Areas.Admin.Controllers
{
    [AdminAuthorize] // Chỉ Admin mới được vào đây
    public class UserController : Controller
    {
        private WebViecLamContext db = new WebViecLamContext();

        // GET: Admin/User
        public ActionResult Index()
        {
            // Lấy toàn bộ danh sách người dùng, loại trừ tài khoản Admin hiện tại để tránh tự xóa mình
            var currentUserEmail = Session["Email"]?.ToString();
            var users = db.Users.Where(u => u.Email != currentUserEmail).ToList();
            return View(users);
        }

        // Chức năng Xóa người dùng
        public ActionResult Delete(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                // Trước khi xóa User, bạn có thể cần xóa các dữ liệu liên quan 
                // (Ví dụ: Jobs của Employer đó) nếu database không để Cascade Delete.
                db.Users.Remove(user);
                db.SaveChanges();
                TempData["SuccessMsg"] = "Đã xóa người dùng thành công!";
            }
            return RedirectToAction("Index");
        }
    }
}