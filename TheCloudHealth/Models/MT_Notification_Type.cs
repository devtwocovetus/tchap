using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Notification_Type
    {
        [FirestoreProperty]
        public string NT_Unique_ID { get; set; }
        [FirestoreProperty]
        public string NT_Category_ID { get; set; }
        [FirestoreProperty]
        public string NT_Category_Name { get; set; }
        [FirestoreProperty]
        public string NT_Category_Type_Code { get; set; }
        [FirestoreProperty]
        public string NT_Category_Type_Name { get; set; }
        [FirestoreProperty]
        public string NT_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string NT_Office_Type { get; set; }
        [FirestoreProperty]
        public string NT_Created_By { get; set; }
        [FirestoreProperty]
        public string NT_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime NT_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime NT_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean NT_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean NT_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string NT_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class NotiTypeResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_Notification_Type Data { get; set; }
        public List<MT_Notification_Type> DataList { get; set; }
    }
}