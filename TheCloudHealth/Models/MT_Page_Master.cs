using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Page_Master
    {
        [FirestoreProperty]
        public string PM_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Category_Name { get; set; }
        [FirestoreProperty]
        public string Page_Name { get; set; }
        [FirestoreProperty]
        public Boolean Is_View { get; set; }
        [FirestoreProperty]
        public Boolean Is_Add { get; set; }
        [FirestoreProperty]
        public Boolean Is_Edit { get; set; }
        [FirestoreProperty]
        public Boolean Is_Delete { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class PageMasterResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Page_Master Data { get; set; }
        public List<MT_Page_Master> DataList { get; set; }
    }
}