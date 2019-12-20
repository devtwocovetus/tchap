using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_KBCategory
    {
        [FirestoreProperty]
        public string KBC_Unique_ID { get; set; }
        [FirestoreProperty]
        public string KBC_Category { get; set; }
        [FirestoreProperty]
        public string KBC_Sub_Category { get; set; }
        //[FirestoreProperty]
        //public string KBC_Sub_Category_Text { get; set; }
        [FirestoreProperty]
        public string KBC_Description { get; set; }
        [FirestoreProperty]
        public string KBC_Created_By { get; set; }
        [FirestoreProperty]
        public string KBC_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime KBC_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime KBC_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean KBC_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean KBC_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string KBC_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class KBCategoryResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_KBCategory Data { get; set; }
        public List<MT_KBCategory> DataList { get; set; }
    }
}