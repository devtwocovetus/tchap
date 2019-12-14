using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Nationality
    {
        [FirestoreProperty]
        public string Nati_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Nati_Name { get; set; }
        [FirestoreProperty]
        public Boolean Nati_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Nati_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Nati_Create_By { get; set; }
        [FirestoreProperty]
        public string Nati_CB_Name { get; set; }
        [FirestoreProperty]
        public string Nati_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Nati_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Nati_Modify_Date { get; set; }
        [FirestoreProperty]
        public string Nati_User_Name { get; set; }
        [FirestoreProperty]
        public string Nati_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class NationalityResonse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Nationality Data { get; set; }
        public List<MT_Nationality> DataList { get; set; }
    }
}