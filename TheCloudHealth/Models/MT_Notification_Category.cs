using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Notification_Category
    {
        [FirestoreProperty]
        public string NC_Unique_ID { get; set; }
        [FirestoreProperty]
        public string NC_Category_Name { get; set; }
        [FirestoreProperty]
        public string NC_Category_Code { get; set; }
        [FirestoreProperty]
        public string NC_Created_By { get; set; }
        [FirestoreProperty]
        public string NC_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime NC_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime NC_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean NC_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean NC_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string NC_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class NotiCateResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_Notification_Category Data { get; set; }
        public List<MT_Notification_Category> DataList { get; set; }
    }
}