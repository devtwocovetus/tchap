using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_SurgC_Contact_Setting
    {
        [FirestoreProperty]
        public string SCS_Unique_ID { get; set; }
        [FirestoreProperty]
        public string SCS_Contact_Type { get; set; }
        [FirestoreProperty]
        public string SCS_First_Name { get; set; }
        [FirestoreProperty]
        public string SCS_Last_Name { get; set; }
        [FirestoreProperty]
        public string SCS_Email { get; set; }
        [FirestoreProperty]
        public string SCS_Phone_No { get; set; }
        [FirestoreProperty]
        public Boolean SCS_Is_Active{ get; set; }
        [FirestoreProperty]
        public Boolean SCS_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string SCS_Created_By { get; set; }
        [FirestoreProperty]
        public string SCS_User_Name { get; set; }
    }
}