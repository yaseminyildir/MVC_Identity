using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Identity.Areas.Administrator.Controllers
{
    [Area("Administrator")]//controller sınıfının "Administrator" adında bir alana ait olduğunu belirtiyor.
    [Authorize(Roles ="Admin")]// Controller sınıfındaki işlemlere erişimi "Admin" rolüne sahip olan kullanıcılara sınırlar. Yani, sadece "Admin" rolüne sahip kullanıcılar bu Controller sınıfının işlemlerine erişebilir
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
