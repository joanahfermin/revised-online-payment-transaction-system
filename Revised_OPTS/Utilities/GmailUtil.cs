using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Drawing.Imaging;
using System.Configuration;
using Inventory_System.Model;
using Inventory_System.Service;
using Revised_OPTS.Service;

namespace Inventory_System.Utilities
{
    internal class GmailUtil
    {
        private static ISystemService systemService = ServiceFactory.Instance.GetSystemService();

        public static bool SendMail(string recipient, string subject, string body, RPTAttachPicture attachPicture)
        {
            
            EmailAccount emailAccount = systemService.GetEmailAccount();

            string finalEmailBody = body;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            try
            {
                using (SmtpClient smtpClient = new SmtpClient() // prepare connection to Gmail
                {
                    //Port = 587,
                    //Host = "smtp.gmail.com",
                    Port = systemService.GetGmailPort(),
                    Host = systemService.GetGmailHost(),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailAccount.UserName, emailAccount.PassWord),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                })
                using (MailMessage message = new MailMessage() // Prepare the email message we wish to send
                {
                    IsBodyHtml = true,
                    From = new MailAddress(emailAccount.UserName),
                    Subject = subject,
                    Body = finalEmailBody
                })
                {
                    message.To.Add(new MailAddress(recipient));  // add recipient of the email message

                    if (attachPicture != null)  // May attachment na picture
                    {
                        using (var altViewHtml = AlternateView.CreateAlternateViewFromString(finalEmailBody, null, MediaTypeNames.Text.Html))
                        using (var pictureMemoryStream = new MemoryStream(attachPicture.FileData))
                        {
                            //attachImage.Save(pictureMemoryStream, ImageFormat.Jpeg); // prepare picture

                            string fileName = attachPicture.FileName;
                            if (fileName == null || fileName.Trim().Length == 0)
                            {
                                fileName = "or.jpg";
                            }

                            pictureMemoryStream.Position = 0;

                            message.AlternateViews.Add(altViewHtml);
                            Attachment att = new Attachment(pictureMemoryStream, fileName);
                            message.Attachments.Add(att);
                            smtpClient.Send(message); // padala na kasi naka attach na ang pix
                        }
                    }

                    else
                    {
                        smtpClient.Send(message); // wala attachment, send lang derecho ang email
                    }
                }

                return true; // walang error sa pag send ng email, return natin true -> which means success.
            }
            catch (System.Exception ex)
            {
                return false; // may error sa pag send ng email, return natin false -> which means failed.
            }
        }

    }
}
