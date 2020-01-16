using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Reject_Reason
    {
        [FirestoreProperty]
        public string RR_Message { get; set; }
        [FirestoreProperty]
        public string RR_Create_By { get; set; }
        [FirestoreProperty]
        public string RR_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime RR_Create_Date { get; set; }
        [FirestoreProperty]
        public string RR_TimeZone { get; set; }
    }
}