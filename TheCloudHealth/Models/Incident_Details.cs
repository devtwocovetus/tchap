using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Incident_Details
    {
        [FirestoreProperty]
        public string Inci_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Inci_Type_ID { get; set; }
        [FirestoreProperty]
        public string Inci_Type { get; set; }
        [FirestoreProperty]
        public string Inci_Claim_No { get; set; }
        [FirestoreProperty]
        public DateTime Inci_DOI { get; set; }
        [FirestoreProperty]
        public string Inci_Employee_Name { get; set; }
        [FirestoreProperty]
        public string Inci_Employee_Address { get; set; }
        [FirestoreProperty]
        public string Inci_Employee_Phone_No { get; set; }
        [FirestoreProperty]
        public Boolean Inci_Is_This_Lien { get; set; }
        [FirestoreProperty]
        public string Inci_Attorney_Name { get; set; }
        [FirestoreProperty]
        public string Inci_Attorney_Phone_No { get; set; }
        [FirestoreProperty]
        public string Inci_TimeOfIncident { get; set; }
        [FirestoreProperty]
        public string Inci_Comment { get; set; }
        [FirestoreProperty]
        public string Inci_Created_By { get; set; }
        [FirestoreProperty]
        public string Inci_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime Inci_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Inci_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Inci_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Inci_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Inci_TimeZone { get; set; }
    }
}