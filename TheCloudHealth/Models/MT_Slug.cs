using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Slug
    {
        [FirestoreProperty]
        public string Slug_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Slug_Name { get; set; }
        [FirestoreProperty]
        public string Slug_SCPO_ID { get; set; }
        [FirestoreProperty]
        public string Slug_Office_Type { get; set; }
        [FirestoreProperty]
        public Boolean Slug_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Slug_Is_Deleted { get; set; }

    }
    public class SlugMResponse
    {
    public string Message { get; set; }
    public int Status { get; set; }
    public MT_Slug Data { get; set; }
}
}