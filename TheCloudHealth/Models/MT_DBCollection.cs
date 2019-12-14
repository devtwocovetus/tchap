using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_DBCollection
    {
        [FirestoreProperty]
        public string DBC_Unique_ID { get; set; }
        [FirestoreProperty]
        public string DBC_File_Name { get; set; }
        [FirestoreProperty]
        public string DBC_File_Path { get; set; }
        [FirestoreProperty]
        public string DBC_Project_ID { get; set; }
        [FirestoreProperty]
        public string DBC_Created_By { get; set; }
        [FirestoreProperty]
        public string DBC_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime DBC_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime DBC_Modify_Date { get; set; }
        [FirestoreProperty]
        public string DBC_TimeZone { get; set; }
        [FirestoreProperty]
        public Boolean DBC_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean DBC_Is_Deleted { get; set; }
    }

    public class DBCollectionResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_DBCollection Data { get; set; }
        public List<MT_DBCollection> DataList { get; set; }
    }
}