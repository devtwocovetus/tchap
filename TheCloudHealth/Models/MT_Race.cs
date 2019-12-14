using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Race
    {
        [FirestoreProperty]
        public string Race_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Race_Type { get; set; }
        [FirestoreProperty]
        public string Race_Name { get; set; }
        [FirestoreProperty]
        public Boolean Race_Is_Active { get; set; }
        [FirestoreProperty]
        public string Race_Create_By { get; set; }
        [FirestoreProperty]
        public string Race_CB_Name { get; set; }
        [FirestoreProperty]
        public string Race_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Race_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Race_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Race_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Race_User_Name { get; set; }
        [FirestoreProperty]
        public string Race_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class RaceResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Race Data { get; set; }
        public List<MT_Race> DataList { get; set; }
    }
}