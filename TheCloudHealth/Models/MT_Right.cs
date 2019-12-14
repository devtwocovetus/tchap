using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Right
    {
        [FirestoreProperty]
        public string RM_Unique_ID { get; set; }
        [FirestoreProperty]
        public string RM_Category_Name { get; set; }
        [FirestoreProperty]
        public string RM_Sub_Category_Name { get; set; }
        [FirestoreProperty]
        public string RM_Page_Name { get; set; }
        [FirestoreProperty]
        public Boolean RM_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean RM_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string RM_Created_By { get; set; }
        [FirestoreProperty]
        public string RM_CB_Name { get; set; }
        [FirestoreProperty]
        public string RM_Created_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime RM_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime RM_Modify_Date { get; set; }
        [FirestoreProperty]
        public string RM_User_Name { get; set; }
        [FirestoreProperty]
        public MT_Right_Details RM_Right_Details { get; set; }
        [FirestoreProperty]
        public string RM_TimeZone { get; set; }
        [FirestoreProperty]
        public string Project_ID { get; set; }
    }
    public class RightResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Right Data { get; set; }
        public List<MT_Right> DataList { get; set; }
    }
}