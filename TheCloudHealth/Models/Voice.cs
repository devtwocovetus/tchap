using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    public class Voice
    {
        public string Voice_Receiver_Contact_No { get; set; }
        public string Voice_Call_Body { get; set; }
        public DateTime Voice_Call_Date { get; set; }
        public string Voice_Call_Title { get; set; }        
    }
    public class VoiResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
    }
}