using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Notification_Action
    {
        [FirestoreProperty]
        public string NFA_Unique_ID { get; set; }
        [FirestoreProperty]
        public string NFA_Action_Type { get; set; }
        [FirestoreProperty]
        public string NFA_Action_Title { get; set; }
        [FirestoreProperty]
        public string NFA_Action_Subject { get; set; }
        [FirestoreProperty]
        public string NFA_Action_Icon { get; set; }
        [FirestoreProperty]
        public int NFA_Be_Af { get; set; }
        [FirestoreProperty]
        public int NFA_Days { get; set; }
        [FirestoreProperty]
        public int NFA_DayOrWeek { get; set; }
        [FirestoreProperty]
        public string NFA_Message { get; set; }
        [FirestoreProperty]
        public string NFA_Timing { get; set; }
        [FirestoreProperty]
        public string NFA_Created_By { get; set; }
        [FirestoreProperty]
        public string NFA_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime NFA_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime NFA_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean NFA_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean NFA_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string NFA_TimeZone { get; set; }
        [FirestoreProperty]
        public string NFA_Status { get; set; }
    }
}