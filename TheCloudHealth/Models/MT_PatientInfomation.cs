using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_PatientInfomation
    {
        [FirestoreProperty]
        public string Patient_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Patient_Code { get; set; }
        [FirestoreProperty]
        public string Patient_Prefix { get; set; }
        [FirestoreProperty]
        public string Patient_First_Name { get; set; }
        [FirestoreProperty]
        public string Patient_Middle_Name { get; set; }
        [FirestoreProperty]
        public string Patient_Last_Name { get; set; }
        [FirestoreProperty]
        public DateTime Patient_DOB { get; set; }
        [FirestoreProperty]
        public string Patient_Sex { get; set; }
        [FirestoreProperty]
        public string Patient_SSN { get; set; }
        [FirestoreProperty]
        public string Patient_Address1 { get; set; }
        [FirestoreProperty]
        public string Patient_Address2 { get; set; }
        [FirestoreProperty]
        public string Patient_City { get; set; }
        [FirestoreProperty]
        public string Patient_State { get; set; }
        [FirestoreProperty]
        public string Patient_Zipcode { get; set; }
        [FirestoreProperty]
        public string Patient_Primary_No { get; set; }
        [FirestoreProperty]
        public string Patient_Secondary_No { get; set; }
        [FirestoreProperty]
        public string Patient_Spouse_No { get; set; }
        [FirestoreProperty]
        public string Patient_Work_No { get; set; }
        [FirestoreProperty]
        public string Patient_Emergency_No { get; set; }
        [FirestoreProperty]
        public string Patient_Email { get; set; }
        [FirestoreProperty]
        public string Patient_Religion { get; set; }
        [FirestoreProperty]
        public string Patient_Ethinicity { get; set; }
        [FirestoreProperty]
        public string Patient_Race { get; set; }
        [FirestoreProperty]
        public string Patient_Marital_Status { get; set; }
        [FirestoreProperty]
        public string Patient_Nationality { get; set; }
        [FirestoreProperty]
        public string Patient_Language { get; set; }        
        [FirestoreProperty]
        public string Patient_Height_In_Ft { get; set; }
        [FirestoreProperty]
        public string Patient_Height_In_Inch { get; set; }
        [FirestoreProperty]
        public string Patient_Weight { get; set; }
        [FirestoreProperty]
        public string Patient_Body_Mass_Index { get; set; }
        [FirestoreProperty]
        public string Patient_Data { get; set; }
        [FirestoreProperty]
        public string Patient_Insurance_Type { get; set; }
        [FirestoreProperty]
        public string Patient_Response_Data { get; set; }
        [FirestoreProperty]
        public Patient_Primary_Insurance_Details Patient_Primary_Insurance_Details { get; set; }
        [FirestoreProperty]
        public Patient_Secondary1_Insurance_Details Patient_Secondary1_Insurance_Details { get; set; }
        [FirestoreProperty]
        public Patient_Secondary2_Insurance_Details Patient_Secondary2_Insurance_Details { get; set; }
        [FirestoreProperty]
        public Patient_Secondary3_Insurance_Details Patient_Secondary3_Insurance_Details { get; set; }
        [FirestoreProperty]
        public string Patient_Surgery_Physician_Center_ID { get; set; }
        [FirestoreProperty]
        public string Patient_Office_Type { get; set; }
        [FirestoreProperty]
        public string Patient_Created_By { get; set; }
        [FirestoreProperty]
        public string Patient_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime Patient_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Patient_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Patient_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Patient_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Patient_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }

    }

    public class PatientInfoResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public string DocPath { get; set; }
        public string DataString { get; set; }
        public MT_PatientInfomation Data { get; set; }
        public List<MT_PatientInfomation> DataList { get; set; }
    }

    public class PatientEResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public Boolean Is_Available { get; set; }        
    }
}