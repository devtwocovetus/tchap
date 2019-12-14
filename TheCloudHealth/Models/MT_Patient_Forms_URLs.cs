using Google.Cloud.Firestore;
using System;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Patient_Forms_URLs
    {
        [FirestoreProperty]
        public string PFU_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PFU_Booking_ID { get; set; }
        [FirestoreProperty]
        public string PFU_Actual_URL { get; set; }
        [FirestoreProperty]
        public string PFU_Dummy_URL { get; set; }
        [FirestoreProperty]
        public Boolean PFU_Is_Active { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class PFURLResponse 
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public string AURL { get; set; }
        public string DURL { get; set; }
        public MT_Patient_Forms_URLs Data { get; set; }
    }
}