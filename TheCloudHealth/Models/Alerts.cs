using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Alerts
    {
        [FirestoreProperty]
        public string Alrt_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Alrt_Name { get; set; }
        [FirestoreProperty]
        public Boolean Alrt_Is_Needed { get; set; }
    }
}