using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Anesthesia
    {
        [FirestoreProperty]
        public string Anes_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Anes_Type { get; set; }
        [FirestoreProperty]
        public string Anes_Name { get; set; }
        [FirestoreProperty]
        public string Anes_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string Anes_Office_Type { get; set; }
        [FirestoreProperty]
        public string Anes_Create_By { get; set; }
        [FirestoreProperty]
        public string Anes_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Anes_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Anes_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Anes_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Anes_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Anes_User_Name { get; set; }
        [FirestoreProperty]
        public string Anes_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }

    }
    public class AnesthesiaResponse
    {

        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Anesthesia Data { get; set; }
        public List<MT_Anesthesia> DataList { get; set; }
    }
}