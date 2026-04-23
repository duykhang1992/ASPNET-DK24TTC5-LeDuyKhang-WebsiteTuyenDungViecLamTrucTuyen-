using System.Linq;
using System.Web.Mvc;

namespace WebViecLam.Models
{
    public class HomeController : Controller
    {
        private WebViecLamContext db = new WebViecLamContext();

        public ActionResult Index()
        {
            // Chỉ lấy 4 hoặc 6 việc làm mới nhất để hiển thị ở Trang chủ
            var latestJobs = db.Jobs.OrderByDescending(j => j.CreatedDate).Take(4).ToList();

            // Truyền danh sách này ra View
            return View(latestJobs);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}