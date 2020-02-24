using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Patient_Intake
    {
        [FirestoreProperty]
        public string PIT_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PIT_Category_ID { get; set; }
        [FirestoreProperty]
        public string PIT_Category_Name { get; set; }
        [FirestoreProperty]
        public string PIT_Category_Type_ID { get; set; }
        [FirestoreProperty]
        public string PIT_Name { get; set; }
        [FirestoreProperty]
        public string PIT_Description { get; set; }
        [FirestoreProperty]
        public string PIT_Category_Type_Name { get; set; }
        [FirestoreProperty]
        public List<Notification_Action> PIT_Actions { get; set; }
        [FirestoreProperty]
        public string PIT_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string PIT_Office_Type { get; set; }
        [FirestoreProperty]
        public string PIT_Created_By { get; set; }
        [FirestoreProperty]
        public string PIT_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime PIT_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime PIT_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean PIT_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean PIT_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string PIT_TimeZone { get; set; }
        [FirestoreProperty]
        public string PIT_Status { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class PatientIntakeResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_Patient_Intake Data { get; set; }
        public List<MT_Patient_Intake> DataList { get; set; }
    }
}