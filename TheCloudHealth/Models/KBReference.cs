using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class KBReference
    {
        [FirestoreProperty]
        public string KBR_Unique_ID { get; set; }
        [FirestoreProperty]
        public string KBR_Description { get; set; }
        [FirestoreProperty]
        public string KBR_Link { get; set; }
        [FirestoreProperty]
        public Boolean KBR_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean KBR_Is_Deleted { get; set; }
        [FirestoreProperty]
        public DateTime KBR_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime KBR_Modify_Date { get; set; }
        [FirestoreProperty]
        public string KBR_TimeZone { get; set; }
    }
}