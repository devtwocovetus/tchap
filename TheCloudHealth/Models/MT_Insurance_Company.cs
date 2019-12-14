using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Insurance_Company
    {
        [FirestoreProperty]
        public string INC_Unique_ID { get; set; }
        [FirestoreProperty]
        public string INC_WS_ID { get; set; }
        [FirestoreProperty]
        public string INC_Company_Name { get; set; }
        [FirestoreProperty]
        public string INC_Created_By { get; set; }
        [FirestoreProperty]
        public string INC_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime INC_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime INC_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean INC_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean INC_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string INC_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class InsurCompanyResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public MT_Insurance_Company Data { get; set; }
        public List<MT_Insurance_Company> DataList { get; set; }
    }
}