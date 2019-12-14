using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Pack
    {
        [FirestoreProperty]
        public string Pack_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Pack_Name { get; set; }
        [FirestoreProperty]
        public string Pack_Created_By { get; set; }
        [FirestoreProperty]
        public string Pack_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime Pack_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Pack_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Pack_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Pack_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Pack_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class PackResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Pack Data { get; set; }
        public List<MT_Pack> DataList { get; set; }
    }
}