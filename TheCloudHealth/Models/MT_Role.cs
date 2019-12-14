using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Role
    {
        [FirestoreProperty]
        public string ROM_Unique_ID { get; set; }
        [FirestoreProperty]
        public string ROM_Name { get; set; }
        [FirestoreProperty]
        public string ROM_Description { get; set; }
        [FirestoreProperty]
        public string ROM_Surgery_Physician_Center_ID { get; set; }
        [FirestoreProperty]
        public string ROM_Office_Type { get; set; }
        [FirestoreProperty]
        public string ROM_Created_By { get; set; }
        [FirestoreProperty]
        public string ROM_CB_Name { get; set; }
        [FirestoreProperty]
        public string ROM_Created_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime ROM_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime ROM_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean ROM_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean ROM_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string ROM_User_Name { get; set; }
        [FirestoreProperty]
        public List<MT_User_Right_Priviliages> ROM_Priviliages { get; set; }
        [FirestoreProperty]
        public MT_User_Right_Priviliages[] ROM_Priviliages_Array { get; set; }
        [FirestoreProperty]
        public string ROM_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class RoleResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Role Data { get; set; }
        public List<MT_Role> DataList { get; set; }
    }
}