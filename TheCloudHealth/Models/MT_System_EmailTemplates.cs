using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_System_EmailTemplates
    {
        [FirestoreProperty]
        public string SET_Unique_ID { get; set; }
        [FirestoreProperty]
        public string SET_Name { get; set; }
        [FirestoreProperty]
        public string SET_Description { get; set; }
        [FirestoreProperty]
        public string SET_From_Email { get; set; }
        [FirestoreProperty]
        public string SET_From_Name { get; set; }
        [FirestoreProperty]
        public string SET_CC { get; set; }
        [FirestoreProperty]
        public string SET_Header { get; set; }
        [FirestoreProperty]
        public string SET_Message { get; set; }
        [FirestoreProperty]
        public string SET_Footer { get; set; }
        [FirestoreProperty]
        public string SET_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string SET_Office_Type { get; set; }
        [FirestoreProperty]
        public string SET_Created_By { get; set; }
        [FirestoreProperty]
        public string SET_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime SET_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime SET_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean SET_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean SET_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string SET_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class SETemplatesResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_System_EmailTemplates Data { get; set; }
        public List<MT_System_EmailTemplates> DataList { get; set; }
    }
}