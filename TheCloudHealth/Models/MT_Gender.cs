using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Gender
    {
        [FirestoreProperty]
        public string Gen_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Gen_Type { get; set; }
        [FirestoreProperty]
        public string Gen_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string Gen_Office_Type { get; set; }
        [FirestoreProperty]
        public Boolean Gen_Is_Active { get; set; }
        [FirestoreProperty]
        public string Gen_Create_By { get; set; }
        [FirestoreProperty]
        public string Gen_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Gen_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Gen_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Gen_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Gen_User_Name { get; set; }
    }
}