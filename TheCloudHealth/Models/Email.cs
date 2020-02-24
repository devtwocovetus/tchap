using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    public class Email
    {
        //public EmailAddress From { get; set; }
        //public EmailAddress To { get; set; }
        public string From_Email { get; set; }
        public string From_Name { get; set; }
        public List<string> To_Email { get; set; }
        public List<string> To_Name { get; set; }
        public List<string> Bcc_Email { get; set; }
        public List<string> Cc_Email { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
    }
    public class EmailResponse {
        public string Message { get; set; }
        public int Status { get; set; }
    }
}