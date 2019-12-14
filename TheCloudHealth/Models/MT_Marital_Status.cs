using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Marital_Status
    {
        [FirestoreProperty]
        public string Mari_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Mari_Type { get; set; }
        [FirestoreProperty]
        public Boolean Mari_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Mari_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Mari_Create_By { get; set; }
        [FirestoreProperty]
        public string Mari_CB_Name { get; set; }
        [FirestoreProperty]
        public string Mari_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Mari_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Mari_Modify_Date { get; set; }
        [FirestoreProperty]
        public string Mari_User_Name { get; set; }
        [FirestoreProperty]
        public string Mari_TimeZone { get; set; }
    }
}