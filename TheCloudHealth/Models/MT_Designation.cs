using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Designation
    {
        [FirestoreProperty]
        public string Desi_UniqueID { get; set; }
        [FirestoreProperty]
        public string Desi_Name { get; set; }
        [FirestoreProperty]
        public string Desi_Description { get; set; }
        [FirestoreProperty]
        public string Desi_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string Desi_Office_Type { get; set; }
        [FirestoreProperty]
        public Boolean Desi_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Desi_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Desi_Created_By { get; set; }
        [FirestoreProperty]
        public string Desi_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime Desi_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Desi_Modify_Date { get; set; }
        [FirestoreProperty]
        public string Desi_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }

    }
    public class DesignationResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Designation Data { get; set; }
        public List<MT_Designation> DataList { get; set; }
    }
}