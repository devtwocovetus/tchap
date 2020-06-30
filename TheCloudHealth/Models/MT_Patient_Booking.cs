using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Patient_Booking
    {
        [FirestoreProperty]
        public string PB_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PB_Patient_ID { get; set; }
        [FirestoreProperty]
        public string PB_Patient_Name { get; set; }
        [FirestoreProperty]
        public string PB_Patient_Last_Name { get; set; }
        [FirestoreProperty]
        public string PB_Patient_Email { get; set; }
        [FirestoreProperty]
        public DateTime PB_Booking_Date { get; set; }
        [FirestoreProperty]
        public string PB_Booking_Time { get; set; }
        [FirestoreProperty]
        public string PB_Booking_No { get; set; }
        [FirestoreProperty]
        public string PB_Booking_Duration { get; set; }
        [FirestoreProperty]
        public MT_PatientInfomation PB_Patient_Details{ get; set; }
        [FirestoreProperty]
        public Incident_Details PB_Incient_Detail { get; set; }
        [FirestoreProperty]
        public Surgical_Procedure_Information PB_Surgical_Procedure_Information { get; set; }
        [FirestoreProperty]
        public Preoprative_Medical_Clearance PB_Preoprative_Medical_Clearance { get; set; }
        [FirestoreProperty]
        public Special_Request PB_Special_Request { get; set; }
        [FirestoreProperty]
        public List<Alerts> PB_Alerts { get; set; }
        [FirestoreProperty]
        public Insurance_Precertification_Authorization PB_Insurance_Precertification_Authorization { get; set; }
        [FirestoreProperty]
        public List<Doc_Uploaded> PB_Doc_Uploaded_List { get; set; }
        [FirestoreProperty]
        public List<Patient_Forms> PB_Forms { get; set; }
        [FirestoreProperty]
        public List<MT_Notifications> PB_Notifications { get; set; }
        [FirestoreProperty]
        public string[] PB_Notifications_Array { get; set; }
        [FirestoreProperty]
        public string PB_Booking_Surgery_Center_ID { get; set; }
        [FirestoreProperty]
        public string PB_Booking_Surgery_Center_Name { get; set; }
        [FirestoreProperty]
        public string PB_Booking_Physician_Office_ID { get; set; }
        [FirestoreProperty]
        public string PB_Booking_Physician_Office_Name { get; set; }
        [FirestoreProperty]
        public string PB_Created_By { get; set; }
        [FirestoreProperty]
        public string PB_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime PB_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime PB_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean PB_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean PB_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string PB_Status { get; set; }
        [FirestoreProperty]
        public string PB_Booked_From { get; set; }
        [FirestoreProperty]
        public string PB_TimeZone { get; set; }
        [FirestoreProperty]
        public string Project_ID { get; set; }
        [FirestoreProperty]
        public List<Reason> PB_Reject { get; set; }
        [FirestoreProperty]
        public List<Reason> PB_Approved { get; set; }
        [FirestoreProperty]
        public List<Reason> PB_Draft { get; set; }
        [FirestoreProperty]
        public List<Reason> PB_Incomplete { get; set; }
        [FirestoreProperty]
        public List<Reason> PB_Complete { get; set; }
        [FirestoreProperty]
        public List<Reason> PB_Noshow { get; set; }
        [FirestoreProperty]
        public List<Reason> PB_Suspended { get; set; }
        [FirestoreProperty]
        public List<Reason> PB_Unapproved { get; set; }
        [FirestoreProperty]
        public List<Reason> PB_Notes { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class BookingResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Patient_Booking Data { get; set; }
        public List<MT_Patient_Booking> DataList { get; set; }
        public List<string> StringList { get; set; }
        public Boolean IsAvailable { get; set; }
    }

    public class CountResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Action_Required { get; set; }
        public int In_Review { get; set; }
        public int Rejected { get; set; }
        public int Draft { get; set; }
        public int InComplete { get; set; }
    }
}