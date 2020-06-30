using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_PatientIntakeTiming
    {
        [FirestoreProperty]
        public string PITT_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PITT_Time { get; set; }
        [FirestoreProperty]
        public string PITT_Created_By { get; set; }
        [FirestoreProperty]
        public string PITT_User_Name { get; set; }
        [FirestoreProperty]
        public string PITT_Surgery_Physician_Center_ID { get; set; }
        [FirestoreProperty]
        public string PITT_Office_Type { get; set; }
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
    public class PatInTimingResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_PatientIntakeTiming Data { get; set; }
        public List<MT_PatientIntakeTiming> DataList { get; set; }
    }
}