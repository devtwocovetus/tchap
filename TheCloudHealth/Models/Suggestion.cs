using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Suggestion
    {
        //[FirestoreProperty]
        //public string SG_Unique_ID { get; set; }
        [FirestoreProperty]
        public Boolean SG_Is_Useful { get; set; }
        [FirestoreProperty]
        public string SG_User_ID { get; set; }
    }
}