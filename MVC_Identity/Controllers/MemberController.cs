using Microsoft.AspNetCore.Mvc;

namespace MVC_Identity.Controllers
{
    //Todo: Kullanıcı işlemleri yapılacak.
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
