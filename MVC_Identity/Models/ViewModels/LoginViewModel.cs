using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.Models.ViewModels
{
    public class LoginViewModel
    {
        //Required:boş olmaması gerektiğini belirten bir veri doğrulama niteliğidir. Bu nitelik, model sınıfının içindeki özelliklerin kullanıcı tarafından doldurulması gerektiğini ve boş bırakılamayacağını belirtir.
        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        public string Password { get; set; }//123456
    }
}
