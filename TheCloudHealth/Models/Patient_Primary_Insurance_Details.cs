using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Patient_Primary_Insurance_Details
    {
        [FirestoreProperty]
        public string PPID_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PPID_Relation_To_Patient { get; set; }
        [FirestoreProperty]
        public string PPID_Subscriber_Name { get; set; }
        [FirestoreProperty]
        public string PPID_Subscriber_SSN_No { get; set; }
        [FirestoreProperty]
        public DateTime PPID_DOB { get; set; }
        [FirestoreProperty]
        public string PPID_Policy_No { get; set; }
        [FirestoreProperty]
        public string PPID_Primary_Insurance { get; set; }
        [FirestoreProperty]
        public string PPID_PO_Box_No { get; set; }
        [FirestoreProperty]
        public string PPID_Address { get; set; }
        [FirestoreProperty]
        public string PPID_Address2 { get; set; }
        [FirestoreProperty]
        public string PPID_State { get; set; }
        [FirestoreProperty]
        public string PPID_City { get; set; }
        [FirestoreProperty]
        public string PPID_Zip_Code { get; set; }
        [FirestoreProperty]
        public string PPID_DocPath { get; set; }
        [FirestoreProperty]
        public DateTime PPID_V_Start_Date { get; set; }
        [FirestoreProperty]
        public DateTime PPID_V_End_Date { get; set; }
        [FirestoreProperty]
        public int PPID_Trace_Number { get; set; }
        [FirestoreProperty]
        public string PPID_Created_By { get; set; }
        [FirestoreProperty]
        public string PPID_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime PPID_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime PPID_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean PPID_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean PPID_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string PPID_TimeZone { get; set; }
    }
}