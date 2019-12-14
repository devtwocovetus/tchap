using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Religion
    {
        [FirestoreProperty]
        public string Reli_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Reli_Name { get; set; }
        [FirestoreProperty]
        public Boolean Reli_Is_Active { get; set; }
        [FirestoreProperty]
        public string Reli_Create_By { get; set; }
        [FirestoreProperty]
        public string Reli_CB_Name { get; set; }
        [FirestoreProperty]
        public string Reli_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Reli_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Reli_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Reli_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Reli_User_Name { get; set; }
        [FirestoreProperty]
        public string Reli_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }

    }
    public class ReligionResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Religion Data { get; set; }
        public List<MT_Religion> DataList { get; set; }
    }
}