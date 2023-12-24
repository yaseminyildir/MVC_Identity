using System.Net;
using System.Net.Mail;

namespace MVC_Identity.Utils.EmailUtils
{
    public class EmailSender
    {
        public static void SendEmail(string email, string subject, string body)
        {
            //Bu kod parçaları, .NET Framework veya .NET Core gibi platformlarda e-posta gönderimi için kullanılır.
            MailMessage sender = new MailMessage();
            sender.From = new MailAddress("yzl3171@outlook.com", "Yzl3171");
            sender.To.Add(email);
            sender.Subject = subject;
            sender.Body = body;

            //Smtp
            //Bu kısımlar e-posta göndermek için gerekli olan e-posta içeriğini ve bilgilerini oluşturur.
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential("yzl3171@outlook.com", "Kadikoy3171--");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Host = "smtp-mail.outlook.com";

            smtpClient.Send(sender);

        }

    }
}
