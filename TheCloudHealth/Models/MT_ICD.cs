using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_ICD
    {
        [FirestoreProperty]
        public string ICD_Unique_ID { get; set; }
        [FirestoreProperty]
        public string ICD_Procedure_Code_Category { get; set; }
        [FirestoreProperty]
        public string ICD_ICD10_PCS_Code { get; set; }
        [FirestoreProperty]
        public string ICD_Code_Description { get; set; }
        [FirestoreProperty]
        public string ICD_Status { get; set; }
        [FirestoreProperty]
        public string ICD_Created_By { get; set; }
        [FirestoreProperty]
        public string ICD_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime ICD_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime ICD_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean ICD_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean ICD_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string ICD_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class ICDResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_ICD Data { get; set; }
        public List<MT_ICD> DataList { get; set; }
    }
}