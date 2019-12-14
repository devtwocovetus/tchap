using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Slider
    {
        [FirestoreProperty]
        public string Slider_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Slider_Title { get; set; }
        [FirestoreProperty]
        public string Slider_Description { get; set; }
        [FirestoreProperty]
        public string Slider_Background_Image { get; set; }
        [FirestoreProperty]
        public string Slider_Slider_Image { get; set; }
        [FirestoreProperty]
        public string Slider_Creted_By { get; set; }
        [FirestoreProperty]
        public string Slider_User_Name { get; set; }
        [FirestoreProperty]
        public Boolean Slider_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Slider_Is_Deleted { get; set; }
        [FirestoreProperty]
        public DateTime Slider_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Slider_Modify_Date { get; set; }
    }
}