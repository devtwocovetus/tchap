using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Right_Details
    {
        [FirestoreProperty]
        public string RD_Unique_ID { get; set; }
        [FirestoreProperty]
        public string RD_Right_Master_ID { get; set; }
        [FirestoreProperty]
        public Boolean RD_Add { get; set; }
        [FirestoreProperty]
        public Boolean RD_Edit { get; set; }
        [FirestoreProperty]
        public Boolean RD_Delete { get; set; }
        [FirestoreProperty]
        public Boolean RD_View { get; set; }
        [FirestoreProperty]        
        public Boolean RD_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean RD_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string RD_Created_By { get; set; }
        [FirestoreProperty]
        public string RD_CB_Name { get; set; }
        [FirestoreProperty]
        public string RD_Created_By_Type { get; set; }
        [FirestoreProperty]
        public string RD_TimeZone { get; set; }

    }
}