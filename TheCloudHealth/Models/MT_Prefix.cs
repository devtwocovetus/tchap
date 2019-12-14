using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Prefix
    {
        [FirestoreProperty]
        public string Pre_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Pre_Type { get; set; }
        [FirestoreProperty]
        public Boolean Pre_Is_Active { get; set; }
        [FirestoreProperty]
        public string Pre_Create_By { get; set; }
        [FirestoreProperty]
        public string Pre_CB_Name { get; set; }
        [FirestoreProperty]
        public string Pre_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Pre_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Pre_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Pre_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Pre_User_Name { get; set; }
    }
}