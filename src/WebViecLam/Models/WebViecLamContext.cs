using System.Data.Entity;

namespace WebViecLam.Models
{
    public class WebViecLamContext : DbContext
    {
        // Tên chuỗi kết nối trong Web.config
        public WebViecLamContext() : base("name=WebViecLamConnString") { }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }
    }
}