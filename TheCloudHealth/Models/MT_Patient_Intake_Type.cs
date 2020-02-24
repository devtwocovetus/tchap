using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Patient_Intake_Type
    {
        [FirestoreProperty]
        public string PITT_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PITT_Category_ID { get; set; }
        [FirestoreProperty]
        public string PITT_Category_Name { get; set; }
        [FirestoreProperty]
        public string PITT_Category_Type_Code { get; set; }
        [FirestoreProperty]
        public string PITT_Category_Type_Name { get; set; }
        [FirestoreProperty]
        public string PITT_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string PITT_Office_Type { get; set; }
        [FirestoreProperty]
        public string PITT_Created_By { get; set; }
        [FirestoreProperty]
        public string PITT_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime PITT_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime PITT_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean PITT_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean PITT_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string PITT_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class PatiIntakeTypeResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_Patient_Intake_Type Data { get; set; }
        public List<MT_Patient_Intake_Type> DataList { get; set; }
    }
}