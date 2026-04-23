using System.Linq;
using System.Web.Mvc;
using WebViecLam.Filters;
using WebViecLam.Models;

namespace WebViecLam.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class JobController : Controller
    {
        private WebViecLamContext db = new WebViecLamContext();

        // Trang danh sách quản lý: Cho phép Admin Xóa/Sửa bất kỳ ai
        public ActionResult Index()
        {
            var jobs = db.Jobs.OrderByDescending(x => x.CreatedDate).ToList();
            return View(jobs); 
        }

        // Chức năng Xóa bài đăng rác
        public ActionResult Delete(int id)
        {
            var job = db.Jobs.Find(id);
            if (job != null)
            {
                db.Jobs.Remove(job);
                db.SaveChanges();
                TempData["SuccessMsg"] = "Đã xóa bài đăng thành công!";
            }
            return RedirectToAction("Index");
        }
    }
}