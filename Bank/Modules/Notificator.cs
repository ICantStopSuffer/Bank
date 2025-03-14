using System.Net.Mail;
using System.Net;

namespace Bank.Modules {
    public class Notificator {
        public void sendMail(string to, string subject, string body) {
            using (var client = new SmtpClient()) {
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("ICantStopSuffering@gmail.com", "yjot yceb vtln wmnx");
                using (var message = new MailMessage(
                    from: new MailAddress("ICantStopSuffering@gmail.com", "Test"),
                    to: new MailAddress(to, "Test")
                    )) {

                    message.Subject = subject;
                    message.Body = body;

                    client.Send(message);
                }
            }
        }
    }
}
