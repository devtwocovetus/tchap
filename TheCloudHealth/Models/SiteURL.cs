using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class SiteURL
    {
        [FirestoreProperty]
        public string Site_Domain { get; set; }
        [FirestoreProperty]
        public string Site_URL { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }
}