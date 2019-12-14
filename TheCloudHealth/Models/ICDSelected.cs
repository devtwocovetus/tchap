using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class ICDSelected
    {
        [FirestoreProperty]
        public string ICD_Code { get; set; }
        [FirestoreProperty]
        public string ICD_Description { get; set; }
    }
}