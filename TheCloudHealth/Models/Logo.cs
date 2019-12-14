using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Logo
    {
        [FirestoreProperty]
        public string Logo_Login_Image { get; set; }
        [FirestoreProperty]
        public string Logo_Navigation_Image { get; set; }
        [FirestoreProperty]
        public string Logo_Fav_Image { get; set; }
    }
}