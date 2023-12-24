using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC_Identity.Models.Context;
using MVC_Identity.Models.Entity;
using MVC_Identity.Models.ViewModels;
using MVC_Identity.Utils.EmailUtils;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MVC_Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        // async: metotların eş zamanlı olarak çalışmasına ve uzun sürecek işlemleri bekletmeden diğer işlemlerin devam etmesine olanak sağlar.
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {


                AppUser user = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),//kullanıcının benzersiz bir kimlik değeri atanır. 
                    Email = registerVM.Email,//registerVM daki özellikleri yönlendirme işlemi
                    UserName = registerVM.Email
                    //oluşturulan user nesnesinin özelliklerini doldurur.
                };



                var result = await _userManager.CreateAsync(user, registerVM.Password);//yeni bir kullanıcı user oluşturulur ve belirtilen şifreyle beraber CreateAsync metodu kullanılarak kaydedilmeye çalışılır. result değişkeni, bu işlemin başarılı olup olmadığını içeren bir dönüş değerini temsil eder.
                _userManager.AddToRoleAsync(user, "Member");//user kullanıcısına "Member" rolü atanmaya çalışılır. Yani, kullanıcı oluşturulduktan sonra bu kullanıcıya belirli bir rol ataması yapılır.





                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);// belirtilen user kullanıcısına ait bir e-posta doğrulama token'ı oluşturur.
                var encodeToken = HttpUtility.UrlEncode(token.ToString());//HttpUtility.UrlEncode metodu: bir string'i URL için güvenli hale getirmek için kullanılır. Özellikle URL içinde kullanılacak olan stringleri uygun formata dönüştürür.

                string confirmationLink = Url.Action("Confirmation", "Account", new { id = user.Id, token = encodeToken }, Request.Scheme);
                /*
                 Url.Action() metodu, bir controller işlemine (action) yönlendiren bir URL oluşturmayı sağlar.
               "Confirmation" ve "Account" parametreleri, oluşturulacak URL'nin gideceği controller ve action'ı belirtir.
               new { id = user.Id, token = encodeToken }, oluşturulan URL'nin içinde query string olarak yer alacak parametreleri belirtir. id kullanıcı kimliği (user.Id) ve token ise önceki adımlarda oluşturulan e-posta doğrulama token'ıdır (encodeToken).
               Request.Scheme, oluşturulan URL'nin şemasını (HTTP veya HTTPS) belirtir. Örneğin, http veya https olarak belirlenir
                 
                 */





                EmailSender.SendEmail(registerVM.Email, "Üyelik Aktivasyon", $"Lütfen linki tıklayın. {confirmationLink}");


                if (result.Succeeded)
                {
                    //TempData, geçici verileri  saklamak için kullanılan bir mekanizmadır.
                    TempData["Success"] = "Kullanıcı başarılı bir şekilde oluşturuldu!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = "bir hata meydana geldi!";
                    return View(registerVM);
                }


            }
            else
            {
                return View(registerVM);
            }

        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)//email ve şifre içeren view modeldir.
        {
            if (ModelState.IsValid)//i giriş bilgileri uygunsa, kullanıcı var mı yok mu kontrol edilecektir.
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                /*
                 _userManager, kullanıcı yönetimi işlemlerini gerçekleştirmek için kullanılan bir örnektir.
                  FindByEmailAsync, e-posta adresine göre kullanıcı arar ve varsa ilgili kullanıcıyı getirir.
                 */

                if (user != null)
                {
                    //cookie tanımlaması gerçekleştircek. Ardından giriş başarılı ise yönlendirme yapıalcak action belirlenecek.

                    //_signInManager.PasswordSignInAsync, kullanıcının şifresini doğrular ve giriş işlemini gerçekleştirir.
                
                   var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                    if (result.Succeeded)    //result.Succeeded, giriş işleminin başarılı olup olmadığını kontrol eder.
                    {
                        return RedirectToAction("Index", "Home");//Eğer giriş başarılıysa, kullanıcı ana sayfaya yönlendirilir.
                    }
                    else
                    {

                        TempData["Error"] = "bir hata meydana geldi!";//Başarısızsa, hata mesajı gösterilir ve kullanıcı tekrar giriş yapma sayfasına yönlendirilir.
                        return View(loginViewModel);
                    }


                }
                else
                {
                    return View(loginViewModel);
                }
            }
            else
            {
                return View(loginViewModel);
            }

        }

        //Bu metod, kullanıcıların oturumlarını sonlandırarak güvenli bir şekilde çıkış yapmalarını sağlar. 
        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
            
        {
            //Bu metod, kullanıcının yetkisi olmayan bir sayfaya veya işleme erişmeye çalıştığında görüntülenecek olan bir "Erişim Reddedildi" sayfasını döndürmeyi amaçlar.
            return View();
        }

        public async Task<IActionResult> Confirmation(string id, string token)//Confirmation adlı metot, kullanıcının e-posta doğrulama işlemini gerçekleştirmek için kullanılır.id ve token parametreleri, e-posta doğrulama işlemi için gereken bilgileri içerir.id, kullanıcı kimliği; token, e-posta doğrulama token'ıdır.

        {
            //kullanıcı var mı?
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var decodeToken = HttpUtility.UrlDecode(token);//HttpUtility.UrlDecode, URL içindeki özel karakterleri orijinal biçimlerine çözmek için kullanılır. Bu durumda, e-posta doğrulama token'ı olan token string'i çözümlenerek (decode edilerek) orijinal haline getirilir
                var result = await _userManager.ConfirmEmailAsync(user, decodeToken);// Bu metod, kullanıcıya ait e-posta adresini ve doğrulama token'ını alır ve eğer token doğruysa kullanıcının e-posta doğrulamasını gerçekleştirir.


                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
            }
            //eğer kullanıcı varsa ilgili kullanıcının EmailConfimation özelliğini true yap.
            return RedirectToAction("Index", "Home");
        }
    }
}
