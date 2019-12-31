using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Knowledger_Base
    {
        [FirestoreProperty]
        public string KNB_Unique_ID { get; set; }
        [FirestoreProperty]
        public string KNB_Name { get; set; }
        [FirestoreProperty]
        public List<string> KNB_Category { get; set; }
        [FirestoreProperty]
        public List<string> KNB_Sub_Category { get; set; }
        [FirestoreProperty]
        public string KNB_Short_Description { get; set; }
        [FirestoreProperty]
        public string KNB_Long_Description { get; set; }
        [FirestoreProperty]
        public List<KBAttachment> KNB_Document { get; set; }
        [FirestoreProperty]
        public List<KBReference> KNB_References { get; set; }
        [FirestoreProperty]
        public List<Suggestion> KNB_Suggestions { get; set; }
        [FirestoreProperty]
        public List<Specilities> KNB_Speciality { get; set; }
        [FirestoreProperty]
        public Boolean KNB_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean KNB_Is_Deleted { get; set; }
        [FirestoreProperty]
        public DateTime KNB_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime KNB_Modify_Date { get; set; }
        [FirestoreProperty]
        public string KNB_Created_By { get; set; }
        [FirestoreProperty]
        public string KNB_User_Name { get; set; }
        [FirestoreProperty]
        public string KNB_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class KnowledgeBaseResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_Knowledger_Base Data { get; set; }
        public List<MT_Knowledger_Base> DataList { get; set; }
    }
}