using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class Surgical_Procedure_Information
    {
        [FirestoreProperty]
        public string SPI_Unique_ID { get; set; }
        [FirestoreProperty]
        public string SPI_Surgery_Center_ID { get; set; }
        [FirestoreProperty]
        public string SPI_Surgery_Center_Name { get; set; }
        [FirestoreProperty]
        public string SPI_Surgeon_ID { get; set; }
        [FirestoreProperty]
        public string SPI_Surgeon_Name { get; set; }
        [FirestoreProperty]
        public string SPI_Assi_Surgeon_ID { get; set; }
        [FirestoreProperty]
        public string SPI_Assi_Surgeon_Name { get; set; }
        [FirestoreProperty]
        public DateTime SPI_Date { get; set; }
        [FirestoreProperty]
        public string SPI_Time { get; set; }
        [FirestoreProperty]
        public string SPI_Duration { get; set; }
        [FirestoreProperty]
        public string SPI_Anesthesia_Type_ID { get; set; }
        [FirestoreProperty]
        public string SPI_Anesthesia_Type { get; set; }
        [FirestoreProperty]
        public string SPI_Block_ID { get; set; }
        [FirestoreProperty]
        public string SPI_Block_Type { get; set; }
        [FirestoreProperty]
        public List<ProcedureSelected> SPI_Procedure_SelectedList { get; set; }
        [FirestoreProperty]
        public List<CPTSelected> SPI_CPT_SelectedList { get; set; }
        [FirestoreProperty]
        public List<ICDSelected> SPI_ICD_SelectedList { get; set; }
        [FirestoreProperty]
        public string SPI_Created_By { get; set; }
        [FirestoreProperty]
        public string SPI_User_Name { get; set; }
        [FirestoreProperty]
        public DateTime SPI_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime SPI_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean SPI_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean SPI_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string SPI_TimeZone { get; set; }

    }
}