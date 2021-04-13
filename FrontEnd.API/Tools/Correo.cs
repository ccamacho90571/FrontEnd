using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace FrontEnd.API.Tools
{
    public static class Correo
    {
        public static bool EnviarCorreo(string Destinatario, string Titulo, string Cuerpo)
        {
          
            SmtpClient smtp = new SmtpClient("smtp.gmail.com",587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("no.reply.crpass@gmail.com", "CRPASS2021");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            

            
           

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("no.reply.crpass@gmail.com", "CR Pass");
            mail.To.Add(new MailAddress(Destinatario));
            
            mail.Subject = Titulo;
            mail.IsBodyHtml = true;
            mail.Body = Cuerpo;
            smtp.Send(mail);

            return true;

        }
    }
}
