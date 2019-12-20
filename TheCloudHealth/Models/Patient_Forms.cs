using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Patient_Forms
    {        
        [FirestoreProperty]
        public string PF_Pack_ID { get; set; }
        [FirestoreProperty]
        public string PF_Pack_Name { get; set; }
        [FirestoreProperty]
        public string PF_Form_ID { get; set; }
        [FirestoreProperty]
        public string PF_Form_Name { get; set; }
        [FirestoreProperty]
        public string PF_Created_By { get; set; }
        [FirestoreProperty]
        public string PF_User_Name { get; set; }        
        [FirestoreProperty]
        public DateTime PF_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime PF_Modify_Date { get; set; }
        [FirestoreProperty]
        public string PF_TimeZone { get; set; }
    }
}