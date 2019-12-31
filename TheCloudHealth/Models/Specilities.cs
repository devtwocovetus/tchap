using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Specilities
    {
        [FirestoreProperty]
        public string SPE_Unique_ID { get; set; }
        [FirestoreProperty]
        public string SPE_Name { get; set; }
    }
}