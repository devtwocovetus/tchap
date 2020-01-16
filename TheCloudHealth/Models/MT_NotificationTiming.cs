using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_NotificationTiming
    {
        [FirestoreProperty]
        public string NTT_Unique_ID { get; set; }
        [FirestoreProperty]
        public string NTT_Time { get; set; }
        [FirestoreProperty]
        public string NTT_Created_By { get; set; }
        [FirestoreProperty]
        public string NTT_User_Name { get; set; }
        [FirestoreProperty]
        public string NTT_Surgery_Physician_Center_ID { get; set; }
        [FirestoreProperty]
        public string NTT_Office_Type { get; set; }
        [FirestoreProperty]
        public DateTime NTT_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime NTT_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean NTT_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean NTT_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string NTT_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class NotiTimingResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_NotificationTiming Data { get; set; }
        public List<MT_NotificationTiming> DataList { get; set; }
    }
}