using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WebViecLam.Models;

namespace WebViecLam.Controllers
{
    public class JobController : Controller
    {
        private WebViecLamContext db = new WebViecLamContext();

        // GET: Job
        // Thêm các tham số đầu vào tương ứng với các tiêu chí lọc
        public ActionResult Index(string searchString, string location, int? page)
        {
            // 1. Khởi tạo câu truy vấn lấy toàn bộ công việc (chưa chạy DB ngay)
            var jobs = db.Jobs.AsQueryable();

            // 2. Lọc theo Từ khóa (Tiêu đề hoặc Tên công ty)
            if (!string.IsNullOrEmpty(searchString))
            {
                // Chuyển về chữ thường để tìm kiếm không phân biệt hoa thường (tương đối)
                jobs = jobs.Where(j => j.Title.Contains(searchString) || j.CompanyName.Contains(searchString));
            }

            // 3. Lọc theo Khu vực
            if (!string.IsNullOrEmpty(location))
            {
                jobs = jobs.Where(j => j.Location.Contains(location));
            }

            // 4. Sắp xếp: Luôn đưa tin mới nhất lên đầu
            jobs = jobs.OrderByDescending(j => j.CreatedDate);

            // 5. Lưu lại các giá trị đang tìm kiếm vào ViewBag 
            // Mục đích: Để khi load lại trang, các ô nhập liệu vẫn giữ nguyên chữ người dùng vừa gõ
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentLocation = location;

            // --- LOGIC PHÂN TRANG ---
            int pageSize = 6; // Số lượng bài đăng trên 1 trang
            int pageNumber = (page ?? 1); // Nếu page null thì mặc định là trang 1

            // Thay vì trả về .ToList(), ta trả về .ToPagedList()
            return View(jobs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            // Kiểm tra nếu không có id truyền vào
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm công việc theo id
            Job job = db.Jobs.Find(id);

            // Nếu không tìm thấy công việc trong database
            if (job == null)
            {
                return HttpNotFound();
            }

            return View(job);
        }

        // GET: Job/Create (Hiển thị Form đăng tin)
        public ActionResult Create()
        {
            // Trả về một View rỗng để người dùng nhập liệu
            return View();
        }

        // POST: Job/Create (Xử lý khi bấm nút Lưu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Job job)
        {
            if (ModelState.IsValid)
            {
                // BƯỚC QUAN TRỌNG: Gán UserId của người đang đăng nhập cho bài đăng này
                if (Session["UserId"] != null)
                {
                    job.UserId = (int)Session["UserId"];
                }
                else
                {
                    // Nếu chưa đăng nhập thì không cho đăng
                    return RedirectToAction("Login", "Account");
                }

                job.CreatedDate = DateTime.Now;
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("MyJobs");
            }
            return View(job);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobId,Title,Description,Salary,Location,Deadline,CreatedDate,CompanyName")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Job/Apply/5
        public ActionResult Apply(int? jobId)
        {
            if (jobId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Job job = db.Jobs.Find(jobId);
            if (job == null) return HttpNotFound();

            // Truyền tên công việc ra View để hiển thị cho đẹp
            ViewBag.JobTitle = job.Title;
            ViewBag.CompanyName = job.CompanyName;

            // Gắn sẵn JobId vào Model
            var app = new Application { JobId = job.JobId };
            return View(app);
        }

        // POST: Job/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Apply(Application app, HttpPostedFileBase cvFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng có chọn file chưa
                if (cvFile != null && cvFile.ContentLength > 0)
                {
                    // Lấy đuôi file (vd: .pdf, .docx)
                    string extension = Path.GetExtension(cvFile.FileName).ToLower();

                    // Chỉ cho phép PDF hoặc Word
                    if (extension == ".pdf" || extension == ".doc" || extension == ".docx")
                    {
                        // Đổi tên file để không bị trùng (dùng thời gian thực)
                        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Path.GetFileName(cvFile.FileName);

                        // Thư mục lưu trữ (bạn cần tạo thư mục Uploads/CVs trong project)
                        string path = Path.Combine(Server.MapPath("~/Uploads/CVs"), fileName);

                        // Lưu file lên server
                        cvFile.SaveAs(path);

                        // Lưu đường dẫn vào database
                        app.CVPath = "/Uploads/CVs/" + fileName;

                        // Thêm vào DB và lưu lại
                        db.Applications.Add(app);
                        db.SaveChanges();

                        // Chuyển hướng về trang chủ kèm thông báo thành công (có thể làm trang Success riêng sau)
                        TempData["SuccessMsg"] = "Ứng tuyển thành công!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Chỉ chấp nhận file định dạng PDF hoặc Word (.doc, .docx)");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng đính kèm CV của bạn.");
                }
            }

            // Nếu có lỗi, load lại form và truyền lại thông tin Job
            Job job = db.Jobs.Find(app.JobId);
            ViewBag.JobTitle = job.Title;
            ViewBag.CompanyName = job.CompanyName;
            return View(app);
        }

        // GET: Job/Applications/5 (Xem danh sách CV của một công việc)
        public ActionResult Applications(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm công việc
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách đơn ứng tuyển của công việc này, sắp xếp mới nhất lên đầu
            var applications = db.Applications
                                 .Where(a => a.JobId == id)
                                 .OrderByDescending(a => a.ApplyDate)
                                 .ToList();

            // Truyền tên công việc ra View để hiển thị tiêu đề
            ViewBag.JobTitle = job.Title;

            return View(applications);
        }

        // GET: Job/MyJobs
        public ActionResult MyJobs()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login", "Account");

            // Ép kiểu chắc chắn là int
            int currentUserId = Convert.ToInt32(Session["UserId"]);

            // Debug: Bạn có thể đặt breakpoint ở đây để xem currentUserId có đúng không
            var myJobs = db.Jobs.Where(j => j.UserId == currentUserId).ToList();

            return View(myJobs);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int appId, string status)
        {
            var app = db.Applications.Find(appId);
            if (app != null)
            {
                app.Status = status;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
