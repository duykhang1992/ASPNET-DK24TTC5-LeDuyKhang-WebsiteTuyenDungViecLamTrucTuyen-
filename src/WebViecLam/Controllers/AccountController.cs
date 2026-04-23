using System.Linq;
using System.Web.Mvc;
using WebViecLam.Models;

namespace WebViecLam.Controllers
{
    public class AccountController : Controller
    {
        private WebViecLamContext db = new WebViecLamContext();

        // 1. ĐĂNG KÝ (GET)
        public ActionResult Register()
        {
            return View();
        }

        // 1. ĐĂNG KÝ (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Email đã tồn tại trong Database chưa
                var checkUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (checkUser == null)
                {
                    // Lưu dữ liệu vào DB
                    db.Users.Add(user);
                    db.SaveChanges();

                    TempData["SuccessMsg"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Error = "Email này đã được sử dụng!";
                    return View(user);
                }
            }
            return View(user);
        }

        // 2. ĐĂNG NHẬP (GET)
        public ActionResult Login()
        {
            return View();
        }

        // 2. ĐĂNG NHẬP (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                // Tìm User có khớp Email và Password không
                // Lưu ý: Đồ án môn học nên ta so sánh chuỗi thường. Thực tế phải dùng mã hóa (MD5/Bcrypt).
                var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

                if (user != null)
                {
                    // Lưu thông tin vào Session
                    Session["UserId"] = user.UserId;
                    Session["FullName"] = user.FullName;
                    Session["Role"] = user.Role; // "Employer" hoặc "Candidate"

                    TempData["SuccessMsg"] = "Chào mừng " + user.FullName + " trở lại!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Email hoặc Mật khẩu không đúng!";
                }
            }
            return View();
        }

        // 3. ĐĂNG XUẤT
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa sạch dữ liệu Session
            return RedirectToAction("Index", "Home");
        }
    }
}