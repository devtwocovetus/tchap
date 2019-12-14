using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Footer
    {
        [FirestoreProperty]
        public string Footer_Text { get; set; }
        [FirestoreProperty]
        public Boolean Footer_Is_Show { get; set; }
    }
}