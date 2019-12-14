using Google.Cloud.Firestore;
using System;


namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Insurance_Precertification_Authorization
    {
        [FirestoreProperty]
        public string IPA_Unique_ID { get; set; }
        [FirestoreProperty]
        public string IPA_Insurace_Company_Phone_No { get; set; }
        [FirestoreProperty]
        public string IPA_Insurace_Company_Representative { get; set; }
        [FirestoreProperty]
        public string IPA_Authorization_Name { get; set; }
        [FirestoreProperty]
        public DateTime IPA_DOA { get; set; }
        [FirestoreProperty]
        public string IPA_Created_By { get; set; }
        [FirestoreProperty]
        public string IPA_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime IPA_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime IPA_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean IPA_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean IPA_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string IPA_TimeZone { get; set; }
    }
}