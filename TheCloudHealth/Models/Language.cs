using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Language
    {
        [FirestoreProperty]
        public string Lan_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Lan_Name { get; set; }
        [FirestoreProperty]
        public string Lan_Shotname { get; set; }
        [FirestoreProperty]
        public Boolean Lan_Is_Active { get; set; }
        [FirestoreProperty]
        public string Lan_Create_By { get; set; }
        [FirestoreProperty]
        public string Lan_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Lan_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Lan_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Lan_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Lan_User_Name { get; set; }
        [FirestoreProperty]
        public string Lan_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class LangResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public Language Data { get; set; }
        public List<Language> DataList { get; set; }
    }
}