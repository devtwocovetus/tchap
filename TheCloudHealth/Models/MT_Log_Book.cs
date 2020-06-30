using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Log_Book
    {
        [FirestoreProperty]
        public string Unique_ID { get; set; }
        [FirestoreProperty]
        public string Section_Name { get; set; }
        [FirestoreProperty]
        public string Page_Name { get; set; }
        [FirestoreProperty]
        public string Operation { get; set; }
        [FirestoreProperty]
        public string User_Name { get; set; }
        [FirestoreProperty]
        public string User_ID { get; set; }
        [FirestoreProperty]
        public string Extra_Value { get; set; }
        [FirestoreProperty]
        public DateTime Operation_Time { get; set; }
        [FirestoreProperty]
        public string Ip_Address { get; set; }
        [FirestoreProperty]
        public string TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class LogResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Log_Book Data { get; set; }
        public List<MT_Log_Book> DataList { get; set; }
    }
}