using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Language
    {
        [FirestoreProperty]
        public string Lang_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Lang_Name { get; set; }
        [FirestoreProperty]
        public string Lang_Shotname { get; set; }
        [FirestoreProperty]
        public Boolean Lang_Is_Active { get; set; }
        [FirestoreProperty]
        public string Lang_Create_By { get; set; }
        [FirestoreProperty]
        public string Lang_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Lang_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Lang_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Lang_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Lang_User_Name { get; set; }
        [FirestoreProperty]
        public string Lang_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class LanguageResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Language Data { get; set; }
        public List<MT_Language> DataList { get; set; }
    }
}