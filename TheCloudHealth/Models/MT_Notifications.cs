using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Notifications
    {
        [FirestoreProperty]
        public string NFT_Unique_ID { get; set; }
        [FirestoreProperty]
        public string NFT_Category_ID { get; set; }
        [FirestoreProperty]
        public string NFT_Category_Name { get; set; }
        [FirestoreProperty]
        public string NFT_Category_Type_ID { get; set; }
        [FirestoreProperty]
        public string NFT_Category_Type_Name { get; set; }
        [FirestoreProperty]
        public List<Notification_Action> NFT_Actions { get; set; }
        [FirestoreProperty]
        public string NFT_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string NFT_Office_Type { get; set; }
        [FirestoreProperty]
        public string NFT_Created_By { get; set; }
        [FirestoreProperty]
        public string NFT_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime NFT_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime NFT_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean NFT_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean NFT_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string NFT_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class NotificationsResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_Notifications Data { get; set; }
        public List<MT_Notifications> DataList { get; set; }
    }
}