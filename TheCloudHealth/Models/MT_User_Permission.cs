using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_User_Permission
    {
        [FirestoreProperty]
        public string UP_Unique_ID { get; set; }
        [FirestoreProperty]
        public string User_ID { get; set; }
        [FirestoreProperty]
        public string Page_ID { get; set; }
        [FirestoreProperty]
        public string Category_Name { get; set; }
        [FirestoreProperty]
        public string Page_Name { get; set; }
        [FirestoreProperty]
        public Boolean Is_View { get; set; }
        [FirestoreProperty]
        public Boolean Is_Add { get; set; }
        [FirestoreProperty]
        public Boolean Is_Edit { get; set; }
        [FirestoreProperty]
        public Boolean Is_Delete { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class UserPermissionResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_User_Permission Data { get; set; }
        public List<MT_User_Permission> DataList { get; set; }
    }
}