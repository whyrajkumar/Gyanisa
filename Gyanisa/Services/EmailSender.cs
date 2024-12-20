using Gyanisa.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Gyanisa.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
       
        private readonly EmailConfig emailConfig;
        public IHostingEnvironment _appEnvironment;
        public EmailSender(IOptions<EmailConfig> options, IHostingEnvironment hostingEnvironment)
        {
            this.emailConfig = options.Value;
            _appEnvironment = hostingEnvironment;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            { 
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(emailConfig.FromName, emailConfig.FromAddress));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = message };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    //client.LocalDomain = emailConfig.LocalDomain;
                    // ============Using ssl=false always on local server

                    //await client.ConnectAsync(emailConfig.MailServerAddress, int.Parse(emailConfig.MailServerPort),SecureSocketOptions.Auto).ConfigureAwait(false);
                    await client.ConnectAsync(emailConfig.MailServerAddress,  emailConfig.MailServerPort,false).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential(emailConfig.UserId, emailConfig.UserPassword));
                    //client.Authenticate(emailConfig.UserId, emailConfig.UserPassword);
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMail(string email, string subject, string message)
        {
            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


                var msz = new MailMessage();
                msz.From = new MailAddress("info@tuitioner.in");//Email which you are getting 
                                                                //from contact us page 
                msz.To.Add("mr.rajsharma00@gmail.com");//Where mail will be sent 
                msz.Subject = subject;
                msz.Body = message; 
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "mail.tuitioner.in";
                smtp.Port = 25;
                smtp.Credentials = new System.Net.NetworkCredential
                ("info@tuitioner.in", "11naCg8*");
                smtp.EnableSsl = false;
                smtp.Send(msz);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SendCustomEmailAsync(string email, string subject, string message)
        {
            try
            {

                var msz = new MailMessage();
                msz.From = new MailAddress(emailConfig.FromAddress);//Email which you are getting 
                                                                    //from contact us page 
                msz.To.Add(email);//Where mail will be sent 
                msz.Subject = subject;
                msz.Body = message;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = emailConfig.MailServerAddress;
                smtp.Port = emailConfig.MailServerPort;
                smtp.Credentials = new System.Net.NetworkCredential(emailConfig.UserId, emailConfig.UserPassword);
                smtp.EnableSsl = false;
                smtp.Send(msz);

                //using (var client = new System.Net.Mail.SmtpClient())
                //{
                //    var credential = new NetworkCredential
                //    {
                //        UserName = emailConfig.UserId,
                //        Password = emailConfig.UserPassword
                //    };

                //    client.Credentials = credential;
                //    client.Host = emailConfig.MailServerAddress;
                //    client.Port = emailConfig.MailServerPort;
                //    client.EnableSsl = false;

                //    using (var emailMessage = new MailMessage())
                //    {
                //        emailMessage.From = new MailAddress(Convert.ToString(emailConfig.FromAddress));
                //        emailMessage.To.Add(new MailAddress(email));
                //        emailMessage.Subject = subject;
                //        emailMessage.Body = message;
                //        client.Send(emailMessage);
                //    }
                //}
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string ConfirmEmailRegistrationAsync(ApplicationUser user, string callbackUrl, string subject, string email)
        {
            
            var webRoot = _appEnvironment.WebRootPath;//get wwwroot Folder  
            //Get TemplateFile located at wwwroot/ Templates / EmailTemplate / Register_EmailTemplate.html
            var pathToFile = _appEnvironment.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplate"
                            + Path.DirectorySeparatorChar.ToString()
                            + "Confirmation_Account_Registration.html";

            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            //Email from Email Template  
            string Message = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
            // string body; 
            //string htmlBody = builder.HtmlBody.Replace("{", "{{").Replace("}", "}}");
            string htmlBody = builder.HtmlBody.Replace("{0}", "{"+ user.FirstName + " " + user.LastName +"}")
                                              .Replace("{1}", "{" + callbackUrl + "}")
                                              .Replace("{2}", "{" + emailConfig.LocalDomain + "/Manage/Index" + "}")
                                              .Replace("{3}", "{" + emailConfig.LocalDomain + "/Home/Index" + "}")
                                              ;
            htmlBody = htmlBody.Replace("{", "").Replace("}", "");
            string messageBody = string.Format(htmlBody,
                user.FirstName + user.LastName,
                email,
                "~/Manage/UserProfile/" + user.Id,
                callbackUrl);
            return messageBody;
        }

        public string ConfirmRegistrationAsync112(ApplicationUser user, string callbackUrl, string subject, string email)
        {
            var webRoot = _appEnvironment.WebRootPath;//get wwwroot Folder  
            //Get TemplateFile located at wwwroot/ Templates / EmailTemplate / Register_EmailTemplate.html
            var pathToFile = _appEnvironment.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "Templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "EmailTemplate"
                            + Path.DirectorySeparatorChar.ToString()
                            + "Confirmation_Account_Registration.html";

            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            //Email from Email Template  
            string Message = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
            // string body; 
            string htmlBody = builder.HtmlBody.Replace("{", "{{").Replace("}", "}}");
            string messageBody = string.Format(htmlBody,
                Convert.ToString(user.FirstName + user.LastName),
                email,
                Convert.ToString("~/Manage/UserProfile/" + user.Id),
                callbackUrl);

            //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);
            return messageBody;
        }


        public IEnumerable<string> GetClientIP(string localIP)
        {
            string ip = localIP;

            //https://en.wikipedia.org/wiki/Localhost
            //127.0.0.1    localhost
            //::1          localhost
            if (ip == "::1")
            {
                try
                {
                    ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[2].ToString();
                }
                catch (Exception ex)
                {
                   ip= Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
                }
            }
            return new string[] { ip.ToString() };
        }
    }
}
