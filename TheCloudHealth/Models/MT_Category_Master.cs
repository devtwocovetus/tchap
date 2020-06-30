using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Category_Master
    {
        [FirestoreProperty]
        public string CM_Unique_ID { get; set; }
        [FirestoreProperty]
        public string CM_Login_From { get; set; }
        [FirestoreProperty]
        public List<MT_Category_Detail> CM_Detail { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class CategoryResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Category_Master Data { get; set; }
        public List<MT_Category_Master> DataList { get; set; }
    }
}