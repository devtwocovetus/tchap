using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_User_Right_Priviliages
    {
        [FirestoreProperty]
        public string URP_Role_ID { get; set; }
        [FirestoreProperty]
        public string URP_Right_Mater_ID { get; set; }
        [FirestoreProperty]
        public string URP_Category_Name { get; set; }
        [FirestoreProperty]
        public string URP_Page_Name { get; set; }
        [FirestoreProperty]
        public Boolean URP_View { get; set; }
        [FirestoreProperty]
        public Boolean URP_Add { get; set; }
        [FirestoreProperty]
        public Boolean URP_Edit { get; set; }
        [FirestoreProperty]
        public Boolean URP_Delete { get; set; }
        [FirestoreProperty]
        public string URP_Created_By { get; set; }
        [FirestoreProperty]
        public string URP_User_Name { get; set; }
        [FirestoreProperty]
        public string URP_TimeZone { get; set; }
    }
}