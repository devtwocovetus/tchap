using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_CPT
    {
        [FirestoreProperty]
        public string CPT_Unique_ID { get; set; }
        [FirestoreProperty]
        public string CPT_Procedure_Code_Category { get; set; }
        [FirestoreProperty]
        public string CPT_Code { get; set; }
        [FirestoreProperty]
        public string CPT_Code_Description { get; set; }
        [FirestoreProperty]
        public string CPT_Status { get; set; }
        [FirestoreProperty]
        public string CPT_Created_By { get; set; }
        [FirestoreProperty]
        public string CPT_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime CPT_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime CPT_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean CPT_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean CPT_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string CPT_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class CPTResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_CPT Data { get; set; }
        public List<MT_CPT> DataList { get; set; }
    }
}