using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Procedures
    {
        [FirestoreProperty]
        public string Pro_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Pro_Procedure_Code_Category { get; set; }
        [FirestoreProperty]
        public string Pro_Name { get; set; }
        [FirestoreProperty]
        public string Pro_Description { get; set; }
        [FirestoreProperty]
        public string Pro_Status { get; set; }
        [FirestoreProperty]
        public string Pro_Created_By { get; set; }
        [FirestoreProperty]
        public string Pro_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime Pro_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Pro_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Pro_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Pro_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Pro_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class ProcedureResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Procedures Data { get; set; }
        public List<MT_Procedures> DataList { get; set; }
    }
}