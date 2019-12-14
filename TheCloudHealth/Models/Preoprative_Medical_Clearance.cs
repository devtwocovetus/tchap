using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Preoprative_Medical_Clearance
    {
        [FirestoreProperty]
        public string PMC_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PMC_Is_Require_Pre_Op_Medi_Clearance { get; set; }
        [FirestoreProperty]
        public string PMC_Clearing_Physician_Name { get; set; }
        [FirestoreProperty]
        public string PMC_Is_Require_EKG { get; set; }
        [FirestoreProperty]
        public string PMC_Created_By { get; set; }
        [FirestoreProperty]
        public string PMC_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime PMC_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime PMC_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean PMC_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean PMC_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string PMC_TimeZone { get; set; }
    }
}