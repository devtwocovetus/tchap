using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Alert
    {
        [FirestoreProperty]
        public string Alert_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Alert_Name { get; set; }
        [FirestoreProperty]
        public string Alert_Create_By { get; set; }
        [FirestoreProperty]
        public string Alert_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Alert_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Alert_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Alert_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Alert_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Alert_User_Name { get; set; }
        [FirestoreProperty]
        public string Alert_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class AlertResponse
    {

        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Alert Data { get; set; }
        public List<MT_Alert> DataList { get; set; }
    }
}