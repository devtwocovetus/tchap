using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Incident
    {
        [FirestoreProperty]
        public string Inci_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Inci_Type { get; set; }
        [FirestoreProperty]
        public string Inci_Name { get; set; }
        [FirestoreProperty]
        public Boolean Inci_Is_Active { get; set; }
        [FirestoreProperty]
        public string Inci_Create_By { get; set; }
        [FirestoreProperty]
        public string Inci_CB_Name { get; set; }
        [FirestoreProperty]
        public string Inci_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Inci_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Inci_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Inci_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Inci_User_Name { get; set; }
        [FirestoreProperty]
        public string Inci_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class IncidentResponse {

        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Incident Data { get; set; }
        public List<MT_Incident> DataList { get; set; }
    }
}