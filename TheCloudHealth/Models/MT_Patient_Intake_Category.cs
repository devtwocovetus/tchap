using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Patient_Intake_Category
    {
        [FirestoreProperty]
        public string PITC_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PITC_Category_Name { get; set; }
        [FirestoreProperty]
        public string PITC_Category_Code { get; set; }
        [FirestoreProperty]
        public string PITC_Created_By { get; set; }
        [FirestoreProperty]
        public string PITC_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime PITC_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime PITC_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean PITC_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean PITC_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string PITC_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class PatiIntakeCateResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_Patient_Intake_Category Data { get; set; }
        public List<MT_Patient_Intake_Category> DataList { get; set; }
    }
}