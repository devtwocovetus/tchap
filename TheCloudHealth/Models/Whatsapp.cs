using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    public class Whatsapp
    {
        public string Receiver_Contact_No { get; set; }
        public string Message_Body { get; set; }
        public DateTime Message_Date { get; set; }
        public string Message_Title { get; set; }
    }
    public class WAppResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
    }
}