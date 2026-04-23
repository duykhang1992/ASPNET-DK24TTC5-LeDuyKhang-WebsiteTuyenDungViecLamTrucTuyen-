using System;
using System.Linq;
using System.Web.Mvc;
using WebViecLam.Models;
using WebViecLam.Filters;

namespace WebViecLam.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class DashboardController : Controller
    {
        private WebViecLamContext db = new WebViecLamContext();

        public ActionResult Index()
        {
            // 1. Thống kê con số tổng quát
            ViewBag.TotalJobs = db.Jobs.Count();
            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalApplications = db.Applications.Count();

            // 2. Lấy dữ liệu 7 ngày gần nhất để vẽ biểu đồ
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Now.Date.AddDays(-i))
                .OrderBy(d => d)
                .ToList();

            // Đếm số bài đăng cho mỗi ngày
            var jobCounts = last7Days.Select(day =>
                db.Jobs.Count(j => j.CreatedDate.Day == day.Day && j.CreatedDate.Month == day.Month)
            ).ToList();

            ViewBag.ChartLabels = last7Days.Select(d => d.ToString("dd/MM")).ToList();
            ViewBag.ChartData = jobCounts;

            return View();
        }
    }
}