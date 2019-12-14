using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Instrument
    {
        [FirestoreProperty]
        public string Instru_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Instru_Name { get; set; }
        [FirestoreProperty]
        public string Instru_Type { get; set; }
        [FirestoreProperty]
        public string Instru_Description { get; set; }
        [FirestoreProperty]
        public Boolean Instru_Is_Active { get; set; }
        [FirestoreProperty]
        public string Instru_Create_By { get; set; }
        [FirestoreProperty]
        public string Instru_CB_Name { get; set; }
        [FirestoreProperty]
        public string Instru_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Instru_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Instru_Modify_Date { get; set; }
        [FirestoreProperty]
        public string Instru_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public Boolean Instru_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Instru_User_Name { get; set; }
        [FirestoreProperty]
        public string Instru_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class InstrumentResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Instrument Data { get; set; }
        public List<MT_Instrument> DataList { get; set; }
    }
}