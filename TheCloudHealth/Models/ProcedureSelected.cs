using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class ProcedureSelected
    {
        [FirestoreProperty]
        public string Proc_Code { get; set; }
        [FirestoreProperty]
        public string Proc_Description { get; set; }
    }
}