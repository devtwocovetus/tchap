using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_RelationToParent
    {
        [FirestoreProperty]
        public string Rtop_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Rtop_Type { get; set; }
        [FirestoreProperty]
        public string Rtop_Name { get; set; }
        [FirestoreProperty]
        public Boolean Rtop_Is_Active { get; set; }
        [FirestoreProperty]
        public string Rtop_Create_By { get; set; }
        [FirestoreProperty]
        public string Rtop_CB_Name { get; set; }
        [FirestoreProperty]
        public string Rtop_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Rtop_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Rtop_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Rtop_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Rtop_User_Name { get; set; }
        [FirestoreProperty]
        public string Rtop_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class RelaToParentResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_RelationToParent Data { get; set; }
        public List<MT_RelationToParent> DataList { get; set; }
    }
}