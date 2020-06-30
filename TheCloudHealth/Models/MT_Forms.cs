using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Forms
    {
        [FirestoreProperty]
        public string Form_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Form_Pack_ID { get; set; }
        [FirestoreProperty]
        public string Form_Pack_Name { get; set; }
        [FirestoreProperty]
        public string Form_Name { get; set; }
        [FirestoreProperty]
        public string Form_Description { get; set; }
        [FirestoreProperty]
        public string Form_Data { get; set; }
        [FirestoreProperty]
        public string Form_Signature { get; set; }
        [FirestoreProperty]
        public string Form_Logo { get; set; }
        [FirestoreProperty]
        public string Form_Type { get; set; }
        [FirestoreProperty]
        public string Form_SC_PO_Name { get; set; }
        [FirestoreProperty]
        public string Form_Address { get; set; }
        [FirestoreProperty]
        public string Form_Footer { get; set; }
        [FirestoreProperty]
        public string Form_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string Form_Office_Type { get; set; }
        [FirestoreProperty]
        public string Form_Created_By { get; set; }
        [FirestoreProperty]
        public string Form_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime Form_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Form_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Form_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Form_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Form_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class FormsResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Forms Data { get; set; }
        public List<MT_Forms> DataList { get; set; }
    }
}