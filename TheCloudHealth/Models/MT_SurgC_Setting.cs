using Google.Cloud.Firestore;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_SurgC_Setting
    {
        [FirestoreProperty]
        public string Sett_Unique_ID { get; set; }
        [FirestoreProperty]
        public SiteURL SiteURL { get; set; }
        [FirestoreProperty]
        public Appearance Appearance { get; set; }
        [FirestoreProperty]
        public Logo Logo { get; set; }
        [FirestoreProperty]
        public Slider Slider { get; set; }
        [FirestoreProperty]
        public Footer Footer { get; set; }
        [FirestoreProperty]
        public Miscellaneous Miscellaneous { get; set; }

    }
}