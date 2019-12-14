using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Surgical_Procedure_Templates
    {
        [FirestoreProperty]
        public string SPT_Unique_ID { get; set; }
        [FirestoreProperty]
        public string SPT_Template_Name { get; set; }
        [FirestoreProperty]
        public string SPT_Surgical_Center_Name { get; set; }
        [FirestoreProperty]
        public string SPT_Surgeon_Name { get; set; }
        [FirestoreProperty]
        public string SPT_Co_Surgeon_Name { get; set; }
        [FirestoreProperty]
        public DateTime SPT_Surgery_Date { get; set; }
        [FirestoreProperty]
        public string SPT_Surgery_Time { get; set; }
        [FirestoreProperty]
        public string SPT_Surgery_Duration { get; set; }
        [FirestoreProperty]
        public string SPT_Anesthesia_Type { get; set; }
        [FirestoreProperty]
        public string SPT_Block { get; set; }
        [FirestoreProperty]
        public List<ProcedureSelected> SPT_Procedure_SelectedList { get; set; }
        [FirestoreProperty]
        public List<CPTSelected> SPT_CPT_SelectedList { get; set; }
        [FirestoreProperty]
        public List<ICDSelected> SPT_ICD_SelectedList { get; set; }
        [FirestoreProperty]
        public string SPT_Surgery_Physician_Center_ID { get; set; }
        [FirestoreProperty]
        public string SPT_Surgery_Physician_Center_Name { get; set; }
        [FirestoreProperty]
        public string SPI_Created_By { get; set; }
        [FirestoreProperty]
        public string SPI_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime SPT_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime SPT_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean SPT_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean SPT_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string SPT_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class TemplateResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public Boolean Exist { get; set; }
        public MT_Surgical_Procedure_Templates Data { get; set; }
        public List<MT_Surgical_Procedure_Templates> DataList { get; set; }
    }
}