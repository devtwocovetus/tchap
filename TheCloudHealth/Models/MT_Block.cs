using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Block
    {
        [FirestoreProperty]
        public string Block_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Block_Type { get; set; }
        [FirestoreProperty]
        public string Block_Name { get; set; }
        [FirestoreProperty]
        public Boolean Block_Is_Active { get; set; }
        [FirestoreProperty]
        public string Block_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string Block_Office_Type { get; set; }
        [FirestoreProperty]
        public string Block_Create_By { get; set; }
        [FirestoreProperty]
        public string Block_CB_Name { get; set; }
        [FirestoreProperty]
        public string Block_Create_Name { get; set; }
        [FirestoreProperty]
        public string Block_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Block_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Block_Modify_Date { get; set; }
        [FirestoreProperty]
        public string Block_Anesthesia_Id { get; set; }
        [FirestoreProperty]
        public Boolean Block_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Block_User_Name { get; set; }
        [FirestoreProperty]
        public string Block_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class BlockResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Block Data { get; set; }
        public List<MT_Block> DataList { get; set; }
    }
}