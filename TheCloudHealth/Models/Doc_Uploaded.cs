using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Doc_Uploaded
    {
        [FirestoreProperty]
        public string DU_Unique_ID { get; set; }
        [FirestoreProperty]
        public string DU_Attachment_Type { get; set; }
        [FirestoreProperty]
        public string DU_Doc_Path { get; set; }
        [FirestoreProperty]
        public string DU_Doc_Name { get; set; }
        [FirestoreProperty]
        public string DU_Created_By { get; set; }
        [FirestoreProperty]
        public string DU_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime DU_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime DU_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean DU_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean DU_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string DU_TimeZone { get; set; }
    }
}