using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Localization
    {
        [FirestoreProperty] 
        public string Loc_Unique_ID { get; set; }
        [FirestoreProperty] 
        public string Loc_Resource_ID { get; set; }
        [FirestoreProperty] 
        public string Loc_Value { get; set; }
        [FirestoreProperty] 
        public string Loc_Language_Shortname { get; set; }
        [FirestoreProperty] 
        public string Loc_Resource_Type { get; set; }
        [FirestoreProperty] 
        public DateTime Loc_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Loc_Modify_Date { get; set; }
        [FirestoreProperty]
        public string Loc_TimeZone { get; set; }
        [FirestoreProperty]
        public Boolean Loc_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Loc_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class LocalizationResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Localization Data { get; set; }
        public List<MT_Localization> DataList { get; set; }
    }
}