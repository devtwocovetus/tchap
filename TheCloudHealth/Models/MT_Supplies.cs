using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Supplies
    {
        [FirestoreProperty]
        public string Supp_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Supp_Name { get; set; }
        [FirestoreProperty]
        public string Supp_Type { get; set; }
        [FirestoreProperty]
        public string Supp_Description { get; set; }
        [FirestoreProperty]
        public Boolean Supp_Is_Active { get; set; }
        [FirestoreProperty]
        public string Supp_Create_By { get; set; }
        [FirestoreProperty]
        public string Supp_CB_Name { get; set; }
        [FirestoreProperty]
        public string Supp_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Supp_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Supp_Modify_Date { get; set; }
        [FirestoreProperty]
        public string Supp_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public Boolean Supp_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Supp_User_Name { get; set; }
        [FirestoreProperty]
        public string Supp_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class SuppliesResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Supplies Data { get; set; }
        public List<MT_Supplies> DataList { get; set; }
    }
}