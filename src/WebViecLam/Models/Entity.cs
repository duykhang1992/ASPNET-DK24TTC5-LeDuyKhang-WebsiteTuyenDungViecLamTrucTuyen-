using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebViecLam.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và Tên")]
        public string ApplicantName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        // Trường này lưu đường dẫn tới file PDF/Word lưu trên server
        [Display(Name = "File CV")]
        public string CVPath { get; set; }

        [Display(Name = "Ngày nộp")]
        public DateTime ApplyDate { get; set; } = DateTime.Now;

        // Khóa ngoại liên kết với bảng Job
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
        public string Status { get; set; } = "Đang chờ"; // Mặc định là mới nộp
    }
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [Display(Name = "Tiêu đề công việc")]
        public string Title { get; set; }

        [Display(Name = "Mô tả")]
        [AllowHtml] // Cấp phép cho trường này nhận mã HTML
        public string Description { get; set; }

        [Display(Name = "Mức lương")]
        public string Salary { get; set; }

        [Display(Name = "Địa điểm")]
        public string Location { get; set; }

        [Display(Name = "Hạn nộp hồ sơ")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        [Display(Name = "Ngày đăng")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Giả sử có thêm tên công ty để hiển thị full trên Index
        [Display(Name = "Công ty")]
        public string CompanyName { get; set; }

        // Khóa ngoại liên kết với bảng User
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

        [Display(Name = "Logo Công ty")]
        public string LogoPath { get; set; }
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email đăng nhập")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ tên")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        // Role để phân quyền: "Employer" (Nhà tuyển dụng), "Candidate" (Người tìm việc), "Admin"
        public string Role { get; set; }
    }
}