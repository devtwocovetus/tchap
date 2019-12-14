using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Patient_Secondary2_Insurance_Details
    {
        [FirestoreProperty]
        public string PSID_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PSID_Relation_To_Patient { get; set; }
        [FirestoreProperty]
        public string PSID_Subscriber_Name { get; set; }
        [FirestoreProperty]
        public string PSID_Subscriber_SSN_No { get; set; }
        [FirestoreProperty]
        public DateTime PSID_DOB { get; set; }
        [FirestoreProperty]
        public string PSID_Policy_No { get; set; }
        [FirestoreProperty]
        public string PSID_Primary_Insurance { get; set; }
        [FirestoreProperty]
        public string PSID_PO_Box_No { get; set; }
        [FirestoreProperty]
        public string PSID_Address { get; set; }
        [FirestoreProperty]
        public string PSID_Address2 { get; set; }
        [FirestoreProperty]
        public string PSID_State { get; set; }
        [FirestoreProperty]
        public string PSID_City { get; set; }
        [FirestoreProperty]
        public string PSID_Zip_Code { get; set; }
        [FirestoreProperty]
        public string PSID_DocPath { get; set; }
        [FirestoreProperty]
        public DateTime PSID_V_Start_Date { get; set; }
        [FirestoreProperty]
        public DateTime PSID_V_End_Date { get; set; }
        [FirestoreProperty]
        public int PSID_Trace_Number { get; set; }
        [FirestoreProperty]
        public string PSID_Created_By { get; set; }
        [FirestoreProperty]
        public string PSID_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime PSID_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime PSID_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean PSID_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean PSID_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string PSID_TimeZone { get; set; }
    }
}