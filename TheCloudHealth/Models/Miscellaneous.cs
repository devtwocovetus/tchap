using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Miscellaneous
    {
        [FirestoreProperty]
        public string Misc_GoogleAnalytics_Code { get; set; }
        [FirestoreProperty]
        public string Misc_Header_Code { get; set; }
        [FirestoreProperty]
        public string Misc_BodyStart_Code { get; set; }
        [FirestoreProperty]
        public string Misc_BodyEnd_Code { get; set; }
    }
}