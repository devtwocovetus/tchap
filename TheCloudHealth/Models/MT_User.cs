using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_User
    {
        [FirestoreProperty]
        public string UM_Unique_ID { get; set; }
        [FirestoreProperty]
        public string UM_Username { get; set; }
        [FirestoreProperty]
        public string UM_Password { get; set; }
        [FirestoreProperty]
        public string UM_Email { get; set; }
        [FirestoreProperty]
        public string UM_PhoneNo { get; set; }
        [FirestoreProperty]
        public string UM_Surgary_Physician_CenterID { get; set; }
        [FirestoreProperty]
        public string UM_Office_Type { get; set; }
        [FirestoreProperty]
        public string UM_Office_Type_Description { get; set; }
        [FirestoreProperty]
        public string UM_Role_Type { get; set; }
        [FirestoreProperty]
        public string UM_Created_By { get; set; }
        [FirestoreProperty]
        public string UM_Created_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime UM_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime UM_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean UM_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean UM_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string UM_User_Name { get; set; }
        [FirestoreProperty]
        public string UM_TimeZone { get; set; }
        [FirestoreProperty]
        public string UM_Slug_SC { get; set; }
        [FirestoreProperty]
        public string UM_Slug_PO { get; set; }


    }
    public class UserResponse 
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_User Data { get; set; }
        public List<MT_User> DataList { get; set; }

    }

    public class UserCEmailResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public bool Is_Available { get; set; }
    }
}