using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Staff_Members
    {
        [FirestoreProperty]
        public string Staff_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Staff_Name { get; set; }
        [FirestoreProperty]
        public string Staff_Last_Name { get; set; }
        [FirestoreProperty]
        public string Staff_Email { get; set; }
        [FirestoreProperty]
        public string Staff_PhoneNo { get; set; }
        [FirestoreProperty]
        public string Staff_Mobile { get; set; }
        [FirestoreProperty]
        public string Staff_AlternateNo { get; set; }
        [FirestoreProperty]
        public string Staff_Emergency_ContactNo { get; set; }
        [FirestoreProperty]
        public string Staff_Address1 { get; set; }
        [FirestoreProperty]
        public string Staff_Address2 { get; set; }
        [FirestoreProperty]
        public string Staff_City { get; set; }
        [FirestoreProperty]
        public string Staff_State { get; set; }
        [FirestoreProperty]
        public string Staff_Country { get; set; }
        [FirestoreProperty]
        public string Staff_ZipCode { get; set; }
        [FirestoreProperty]
        public string Staff_Age { get; set; }
        [FirestoreProperty]
        public string Staff_Sex { get; set; }
        [FirestoreProperty]
        public string Staff_DOB { get; set; }
        [FirestoreProperty]
        public string Staff_SSN_No { get; set; }
        [FirestoreProperty]
        public string Staff_DOJ { get; set; }
        [FirestoreProperty]
        public string Staff_Emp_Code { get; set; }
        [FirestoreProperty]
        public string Staff_Doc_Code { get; set; }
        [FirestoreProperty]
        public Boolean Staff_Is_Emp_Code { get; set; }
        [FirestoreProperty]
        public string Staff_Surgery_Physician_Office_ID { get; set; }
        [FirestoreProperty]
        public string Staff_Office_Type { get; set; }
        [FirestoreProperty]
        public string Staff_Designation { get; set; }
        [FirestoreProperty]
        public string Staff_Role_ID { get; set; }
        [FirestoreProperty]
        public string Staff_Role_Name { get; set; }
        [FirestoreProperty]
        public string Staff_Created_By { get; set; }
        [FirestoreProperty]
        public string Staff_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime Staff_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Staff_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Staff_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Staff_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Staff_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }

    }
    public class StaffMResponse {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Staff_Members Data { get; set; }
        public List<MT_Staff_Members> DataList { get; set; }
    }
    public class StaffAResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public bool Is_Available { get; set; }
    }
}