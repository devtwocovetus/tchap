using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Special_Request
    {
        [FirestoreProperty]
        public string SR_Unique_ID { get; set; }
        [FirestoreProperty]
        public string SR_Is_Special_Equip_Req { get; set; }
        [FirestoreProperty]
        public string SR_Equip_ID { get; set; }
        [FirestoreProperty]
        public string SR_Equip_Name { get; set; }
        [FirestoreProperty]
        public string SR_Supplies_ID { get; set; }
        [FirestoreProperty]
        public string SR_Supplies_Name { get; set; }
        [FirestoreProperty]
        public string SR_Instrumentation_ID { get; set; }
        [FirestoreProperty]
        public string SR_Instrumentation_Name { get; set; }
        [FirestoreProperty]
        public string SR_Other { get; set; }
        [FirestoreProperty]
        public string SR_Created_By { get; set; }
        [FirestoreProperty]
        public string SR_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime SR_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime SR_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean SR_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean SR_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string SR_TimeZone { get; set; }
    }
}