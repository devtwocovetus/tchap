using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class CPTSelected
    {
        [FirestoreProperty]
        public string CPTS_Code { get; set; }
        [FirestoreProperty]
        public string CPTS_Description { get; set; }
    }
}