using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;
using TheCloudHealth.Models;

namespace TheCloudHealth.Lib
{
    public class Emailer: IEmailer
    {
        private readonly string _sendGridApiKey;
        public Emailer()
        {
            _sendGridApiKey = "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";
        }

        //public async Task<Response> SendEmail(Email email)
        //{
        //    try
        //    {
        //        var client = new SendGridClient(_sendGridApiKey);   
        //        var msg = MailHelper.CreateSingleEmail(
        //            email.From, email.To, email.Subject, email.PlainTextContent, email.HtmlContent);
        //        var response = await client.SendEmailAsync(msg);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public async Task<Response> SendTestEmail(List<string> recipients, List<string> Bcclist, List<string> Cclist, string subject, string bodyHtml, string sender)
        {
            try
            {
                var client = new SendGridClient(_sendGridApiKey);
                var from = MailHelper.StringToEmailAddress(sender);
                var msg = new SendGridMessage();
                var tos = recipients.Select(x => MailHelper.StringToEmailAddress(x)).ToList();
                msg.SetFrom(from);
                msg.SetGlobalSubject(subject);
                //use htmlcontent only if you are sending only HTML
                if (!string.IsNullOrEmpty(bodyHtml))
                {
                    msg.AddContent(MimeType.Html, bodyHtml);
                }

                //The AddTos method below accepts List<EmailAddress> to add multiple recipients with Personalization

                msg.AddTos(tos);
                if (Cclist != null)
                {
                    if (Cclist.Count > 0)
                    {
                        var Ccs = Cclist.Select(x => MailHelper.StringToEmailAddress(x)).ToList();
                        msg.AddCcs(Ccs);
                    }
                }
                if (Bcclist != null)
                {
                    if (Bcclist.Count > 0)
                    {
                        var Bccs = Bcclist.Select(x => MailHelper.StringToEmailAddress(x)).ToList();
                        msg.AddBccs(Bccs);
                    }
                }

                return await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task<string> Send(Email EML)
        {
            //EML.To = new EmailAddress(EML.To_Email, EML.To_Name);
            //EML.From = new EmailAddress("tch.techconnect@gmail.com", "TheCloudHealth");
            //EML.From = new EmailAddress(EML.From_Email, EML.From_Name);

            //var response = await SendEmail(EML);

            var response = await SendTestEmail(EML.To_Email,EML.Bcc_Email,EML.Cc_Email,EML.Subject,EML.HtmlContent,EML.From_Email);

            var message = "";

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                message = "Thank you for your message, someone will be in touch soon!";
            }
            else
            {
                message = "Your message could not be processed at this time. Please try again later and Your Status code is " + response.StatusCode.ToString();
            }

            return message;
        }
    }
}