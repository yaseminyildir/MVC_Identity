using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Identity.Areas.Administrator.Models.ViewModels;
using MVC_Identity.Models.Context;
using MVC_Identity.Models.Entity;
using System.Reflection;

namespace MVC_Identity.Areas.Administrator.Controllers
{
    [Area("Administrator")]// controller grubunun hangi alana ait olduğunu belirtir.
   // [Authorize]
    public class MemberController : Controller
    {
        // Bu, kullanıcıları yönetmek, kullanıcıları oluşturmak, silmek, düzenlemek ve kimlik doğrulama işlemleri gerçekleştirmek için kullanılır.(bu tanım role ve contex içinde geçerli)
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ProjectContext _context;//veritabanı işlemlerini gerçekleştirmek için kullanır(veri okuma ,yazma,silme,güncelleme...)


        //MemberController sınıfının constructor'ı. Bu constructor, UserManager, RoleManager ve ProjectContext örneklerini alır.
        public MemberController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ProjectContext context)
        {
            
            _userManager = userManager;
           _roleManager = roleManager;
            _context = context;
            // Bu atama işlemi sayesinde bu örnekler, controller içinde kullanılabilir hale gelir.
        }

        public async Task<IActionResult> Index()
        {
        

            // Bu metod, kullanıcıları ve rollerini birleştirerek bir liste oluşturmayı hedefler. Bu liste, kullanıcıların belirli özellikleriyle (Email, PhoneNumber, Address) birlikte rollerini içerecek şekilde hazırlanır ve bir görünüme gönderilerek kullanıcı arayüzünde görüntülenebilir


            var userRoles = from u in _context.Users
                            join ur in _context.UserRoles on u.Id equals ur.UserId
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new UserListViewModel// her bir satırın sonucu bir UserListViewModel nesnesine atanır. Bu nesne, kullanıcıların belirli özelliklerini (Id, Email, PhoneNumber, Address) ve rollerini içerir.
                            {
                                Id = u.Id,
                                Email = u.Email,
                                PhoneNumber = u.PhoneNumber,
                                Address = u.Address,
                                Roles=r
                            };
            



            return View(userRoles.ToList());//userRoles liste haline getirilip viewe gönderilir
        }
    }
}
