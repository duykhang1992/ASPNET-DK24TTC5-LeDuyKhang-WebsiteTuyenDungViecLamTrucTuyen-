namespace WebViecLam.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebViecLam.Models; // Đảm bảo đúng namespace của bạn

    internal sealed class Configuration : DbMigrationsConfiguration<WebViecLam.Models.WebViecLamContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebViecLam.Models.WebViecLamContext context)
        {
            context.Users.AddOrUpdate(u => u.Email,
                new User
                {
                    // Chọn một Email cố định làm Admin
                    Email = "admin@webvieclam.com",
                    Password = "123", // Mật khẩu dễ nhớ để test
                    FullName = "Quản trị viên Hệ thống",
                    Role = "Admin" // QUAN TRỌNG NHẤT: Cấp quyền Admin
                }
            );

            context.Jobs.AddOrUpdate(x => x.JobId,
                new Job
                {
                    JobId = 1,
                    Title = "Lập trình viên ASP.NET Core",
                    CompanyName = "AI Edu",
                    Location = "Đồng Tháp",
                    Salary = "15 - 20 triệu",
                    Description = "Phát triển các hệ thống giáo dục trực tuyến...",
                    Deadline = DateTime.Now.AddDays(30),
                    CreatedDate = DateTime.Now
                },
                new Job
                {
                    JobId = 2,
                    Title = "Chuyên viên Phân tích dữ liệu",
                    CompanyName = "Tech Solutions",
                    Location = "Tiền Giang",
                    Salary = "Thỏa thuận",
                    Description = "Phân tích dữ liệu chuỗi cung ứng nông sản...",
                    Deadline = DateTime.Now.AddDays(15),
                    CreatedDate = DateTime.Now
                },
                new Job
                {
                    JobId = 3,
                    Title = "Quản lý Câu lạc bộ Pickleball",
                    CompanyName = "Green Court",
                    Location = "Vĩnh Long",
                    Salary = "8 - 10 triệu",
                    Description = "Quản lý lịch sân và tổ chức giải đấu...",
                    Deadline = DateTime.Now.AddDays(10),
                    CreatedDate = DateTime.Now
                },
                new Job
                {
                    JobId = 4,
                    Title = "Thiết kế Game (Unity/C#)",
                    CompanyName = "Three Kingdoms Studio",
                    Location = "TP. Hồ Chí Minh",
                    Salary = "25 triệu",
                    Description = "Thiết kế cơ chế game chiến thuật Tam Quốc...",
                    Deadline = DateTime.Now.AddDays(45),
                    CreatedDate = DateTime.Now
                }
            );
        }
    }
}