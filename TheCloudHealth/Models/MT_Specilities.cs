using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Specilities
    {
        [FirestoreProperty]
        public string Spec_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Spec_Name { get; set; }
        [FirestoreProperty]
        public string Spec_Type { get; set; }
        [FirestoreProperty]
        public string Spec_Description { get; set; }
        [FirestoreProperty]
        public string Spec_Create_By { get; set; }
        [FirestoreProperty]
        public string Spec_CB_Name { get; set; }
        [FirestoreProperty]
        public string Spec_Create_By_Type { get; set; }
        [FirestoreProperty]
        public string Spec_User_Name { get; set; }
        [FirestoreProperty]
        public Boolean Spec_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Spec_Is_Deleted { get; set; }
        [FirestoreProperty]
        public DateTime Spec_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Spec_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Spec_Select { get; set; }
        [FirestoreProperty]
        public string Spec_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class SpecilityResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Specilities Data { get; set; }
        public List<MT_Specilities> DataList { get; set; }
    }
}