using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class KBAttachment
    {
        [FirestoreProperty]
        public string KBA_Unique_ID { get; set; }
        [FirestoreProperty]
        public string KBA_Description { get; set; }
        [FirestoreProperty]
        public string KBA_Attachement { get; set; }
        [FirestoreProperty]
        public string KBA_Attachement_Name { get; set; }
        [FirestoreProperty]
        public string KBA_Created_By { get; set; }
        [FirestoreProperty]
        public string KBA_User_Name { get; set; }
        [FirestoreProperty]
        public Boolean KBA_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean KBA_Is_Deleted { get; set; }
        [FirestoreProperty]
        public DateTime KBA_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime KBA_Modify_Date { get; set; }
        [FirestoreProperty]
        public string KBA_TimeZone { get; set; }
    }
}