using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_PhyO_Contact_Setting
    {
        [FirestoreProperty]
        public string PCS_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PCS_Contact_Type { get; set; }
        [FirestoreProperty]
        public string PCS_First_Name { get; set; }
        [FirestoreProperty]
        public string PCS_Last_Name { get; set; }
        [FirestoreProperty]
        public string PCS_Email { get; set; }
        [FirestoreProperty]
        public string PCS_Phone_No { get; set; }
        [FirestoreProperty]
        public Boolean PCS_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean PCS_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string PCS_Created_By { get; set; }
        [FirestoreProperty]
        public string PCS_User_Name { get; set; }
    }
}