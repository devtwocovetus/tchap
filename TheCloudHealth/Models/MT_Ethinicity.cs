using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Ethinicity
    {
        [FirestoreProperty]
        public string Ethi_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Ethi_Name { get; set; }
        [FirestoreProperty]
        public string Ethi_Type { get; set; }
        [FirestoreProperty]
        public string Ethi_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string Ethi_Office_Type { get; set; }
        [FirestoreProperty]
        public Boolean Ethi_Is_Active { get; set; }
        [FirestoreProperty]
        public string Ethi_Create_By { get; set; }
        [FirestoreProperty]
        public string Ethi_CB_Name { get; set; }
        [FirestoreProperty]
        public string Ethi_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Ethi_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Ethi_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Ethi_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Ethi_User_Name { get; set; }
        [FirestoreProperty]
        public string Ethi_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class EthinicityResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Ethinicity Data { get; set; }
        public List<MT_Ethinicity> DataList { get; set; }
    }
}