using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Virtual_Consultant_Booking
    {
        [FirestoreProperty]
        public string VCB_Unique_ID { get; set; }
        [FirestoreProperty]
        public string VCB_Patient_ID { get; set; }
        [FirestoreProperty]
        public string VCB_Patient_Name { get; set; }
        [FirestoreProperty]
        public string VCB_Patient_Last_Name { get; set; }
        [FirestoreProperty]
        public DateTime VCB_Booking_Date { get; set; }
        [FirestoreProperty]
        public List<string> VCB_Booking_Time { get; set; }
        [FirestoreProperty]
        public string VCB_Booking_No { get; set; }
        [FirestoreProperty]
        public string VCB_Doctor_ID { get; set; }
        [FirestoreProperty]
        public string VCB_Doctor_Name { get; set; }
        [FirestoreProperty]
        public List<Doc_Uploaded> VCB_Doc_Uploaded_List { get; set; }        
        [FirestoreProperty]
        public List<MT_Notifications> VCB_Notifications { get; set; }
        [FirestoreProperty]
        public string[] VCB_Notifications_Array { get; set; }
        [FirestoreProperty]
        public string VCB_Booking_Physician_Office_ID { get; set; }
        [FirestoreProperty]
        public string VCB_Booking_Physician_Office_Name { get; set; }
        [FirestoreProperty]
        public string VCB_Created_By { get; set; }
        [FirestoreProperty]
        public string VCB_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime VCB_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime VCB_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean VCB_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean VCB_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string VCB_Status { get; set; }
        [FirestoreProperty]
        public string VCB_Booked_From { get; set; }
        [FirestoreProperty]
        public string VCB_TimeZone { get; set; }
        [FirestoreProperty]
        public List<Reason> VCB_Approved { get; set; }
        [FirestoreProperty]
        public List<Reason> VCB_Draft { get; set; }
        [FirestoreProperty]
        public List<Reason> VCB_Complete { get; set; }
        [FirestoreProperty]
        public List<Reason> VCB_Cancelled { get; set; }        
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class VCBookingResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Virtual_Consultant_Booking Data { get; set; }
        public List<MT_Virtual_Consultant_Booking> DataList { get; set; }
        public List<string> StringList { get; set; }
    }
}