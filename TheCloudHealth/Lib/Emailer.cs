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

        public async Task<Response> SendEmail(Email email)
        {
            try
            {
                var client = new SendGridClient(_sendGridApiKey);
                var msg = MailHelper.CreateSingleEmail(
                    email.From, email.To, email.Subject, email.PlainTextContent, email.HtmlContent);
                var response = await client.SendEmailAsync(msg);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> Send(Email EML)
        {

            EML.To = new EmailAddress(EML.To_Email, EML.To_Name);
            //EML.From = new EmailAddress("tch.techconnect@gmail.com", "TheCloudHealth");
            EML.From = new EmailAddress(EML.From_Email, EML.From_Name);

            var response = await SendEmail(EML);

            var message = "Your message could not be processed at this time. Please try again later.";

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                message = "Thank you for your message, someone will be in touch soon!";
            }
            return message;
        }
    }
}