using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Patient_Form_Data
    {
        [FirestoreProperty]
        public string PFD_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PFD_Booking_ID { get; set; }
        [FirestoreProperty]
        public string PFD_Form_ID { get; set; }
        [FirestoreProperty]
        public List<Form_Element> PFD_Elements { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
    public class PatientFormResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Patient_Form_Data Data { get; set; }
        public List<MT_Patient_Form_Data> DataList { get; set; }
        
    }
}