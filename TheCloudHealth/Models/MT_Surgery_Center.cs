using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Surgery_Center
    {
        [FirestoreProperty]
        public string SurgC_Unique_ID { get; set; }
        [FirestoreProperty]
        public string SurgC_Name { get; set; }
        [FirestoreProperty]
        public string SurgC_DBA_Name { get; set; }
        [FirestoreProperty]
        public string SurgC_Address { get; set; }
        [FirestoreProperty]
        public string SurgC_Address2 { get; set; }
        [FirestoreProperty]
        public string SurgC_City { get; set; }
        [FirestoreProperty]
        public string SurgC_State { get; set; }
        [FirestoreProperty]
        public string SurgC_Country { get; set; }
        [FirestoreProperty]
        public string SurgC_Zip { get; set; }
        [FirestoreProperty]
        public string SurgC_Landline { get; set; }
        [FirestoreProperty]
        public string SurgC_FaxNo { get; set; }
        [FirestoreProperty]
        public string SurgC_Email { get; set; }
        [FirestoreProperty]
        public string SurgC_AlternateNo { get; set; }
        [FirestoreProperty]
        public string SurgC_MobileNo { get; set; }
        [FirestoreProperty]
        public string SurgC_Helpline { get; set; }
        [FirestoreProperty]
        public string SurgC_Website_URL { get; set; }
        [FirestoreProperty]
        public string SurgC_Created_By { get; set; }
        [FirestoreProperty]
        public string SurgC_CB_Name { get; set; }
        [FirestoreProperty]
        public string SurgC_Created_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime SurgC_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime SurgC_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean SurgC_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean SurgC_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string SurgC_User_Name { get; set; }
        [FirestoreProperty]
        public string SurgC_TimeZone { get; set; }
        [FirestoreProperty]
        public string SurgC_Project_ID { get; set; }
        [FirestoreProperty]
        public string[] SurgC_Specilities { get; set; }
        [FirestoreProperty]
        public List<MT_Specilities> SurgC_SpecilitiesList { get; set; }
        [FirestoreProperty]
        public List<MT_SurgC_Contact_Setting> SurgC_ContactSetting { get; set; }
        [FirestoreProperty]
        public SiteURL SurgC_SiteURL { get; set; }
        [FirestoreProperty]
        public Appearance SurgC_Appearance { get; set; }
        [FirestoreProperty]
        public Logo SurgC_Logo { get; set; }
        [FirestoreProperty]
        public Slider SurgC_Slider { get; set; }
        [FirestoreProperty]
        public List<Slider> SurgC_SliderList { get; set; }
        [FirestoreProperty]
        public Footer SurgC_Footer { get; set; }
        [FirestoreProperty]
        public Miscellaneous SurgC_Miscellaneous { get; set; }
        [FirestoreProperty]
        public MT_DBCollection SurgC_DB_Setting { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }



    }
    public class SurgeryCenterResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Surgery_Center Data { get; set; }
        public List<MT_Surgery_Center> DataList { get; set; }
    }

    
}