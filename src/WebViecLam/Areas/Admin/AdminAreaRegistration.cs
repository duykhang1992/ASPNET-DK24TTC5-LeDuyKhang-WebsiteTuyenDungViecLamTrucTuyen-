using System.Web.Mvc;

namespace WebViecLam.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                // QUAN TRỌNG: Phải có Namespace để phân biệt với Controller bên ngoài
                new[] { "WebViecLam.Areas.Admin.Controllers" }
            );
        }
    }
}