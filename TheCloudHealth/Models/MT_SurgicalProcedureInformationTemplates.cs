using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_SurgicalProcedureInformationTemplates
    {
        [FirestoreProperty]
        public string Temp_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Temp_Surgical_Center_ID { get; set; }
        [FirestoreProperty]
        public string Temp_Surgical_Center_Name { get; set; }
        [FirestoreProperty]
        public string Temp_Surgeon_ID { get; set; }
        [FirestoreProperty]
        public string Temp_Surgeon_Name { get; set; }
        [FirestoreProperty]
        public string Temp_Co_Surgeon_ID { get; set; }
        [FirestoreProperty]
        public string Temp_Co_Surgeon_Name { get; set; }
        [FirestoreProperty]
        public string Temp_Anesthesia_Type_ID { get; set; }
        [FirestoreProperty]
        public string Temp_Anesthesia_Type { get; set; }
        [FirestoreProperty]
        public string Temp_Block_ID { get; set; }
        [FirestoreProperty]
        public string Temp_Block_Name { get; set; }
        [FirestoreProperty]
        public List<CPTSelected> Temp_CPT_Selected_List { get; set; }
        [FirestoreProperty]
        public List<ICDSelected> Temp_ICD_Selected_List { get; set; }
        [FirestoreProperty]
        public string Temp_Surgery_Physician_Center_ID { get; set; }
        [FirestoreProperty]
        public string Temp_Office_Type { get; set; }
        [FirestoreProperty]
        public string Temp_Created_By { get; set; }
        [FirestoreProperty]
        public string Temp_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime Temp_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Temp_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean Temp_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean Temp_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Temp_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class SPITemplateResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_SurgicalProcedureInformationTemplates Data { get; set; }
        public List<MT_SurgicalProcedureInformationTemplates> DataList { get; set; }
    }
}