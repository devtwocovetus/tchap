using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Document_Category
    {
        [FirestoreProperty]
        public string DOC_Unique_ID { get; set; }
        [FirestoreProperty]
        public string DOC_Category { get; set; }
        [FirestoreProperty]
        public string DOC_Sub_Category { get; set; }
        [FirestoreProperty]
        public string DOC_Description { get; set; }
        [FirestoreProperty]
        public string Doc_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string Doc_Office_Type { get; set; }
        [FirestoreProperty]
        public string DOC_Created_By { get; set; }
        [FirestoreProperty]
        public string DOC_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime DOC_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime DOC_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean DOC_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean DOC_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string DOC_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class DocuCategoryResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_Document_Category Data { get; set; }
        public List<MT_Document_Category> DataList { get; set; }
    }
}