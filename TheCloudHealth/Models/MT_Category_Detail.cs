using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Category_Detail
    {
        [FirestoreProperty]
        public string CD_Unique_ID { get; set; }
        [FirestoreProperty]
        public string CD_Category_Name { get; set; }
        [FirestoreProperty]
        public Boolean CD_Is_Assigned { get; set; }
    }
}