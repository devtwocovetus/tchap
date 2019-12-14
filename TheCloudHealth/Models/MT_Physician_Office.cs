using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Physician_Office
    {
        [FirestoreProperty]
        public string PhyO_Unique_ID { get; set; }
        [FirestoreProperty]
        public string PhyO_Name { get; set; }
        [FirestoreProperty]
        public string PhyO_DBA_Name { get; set; }
        [FirestoreProperty]
        public string PhyO_Address { get; set; }
        [FirestoreProperty]
        public string PhyO_Address2 { get; set; }
        [FirestoreProperty]
        public string PhyO_Email { get; set; }
        [FirestoreProperty]
        public string PhyO_City { get; set; }
        [FirestoreProperty]
        public string PhyO_State { get; set; }
        [FirestoreProperty]
        public string PhyO_Country { get; set; }
        [FirestoreProperty]
        public string PhyO_Zip { get; set; }
        [FirestoreProperty]
        public string PhyO_Landline { get; set; }
        [FirestoreProperty]
        public string PhyO_FaxNo { get; set; }
        [FirestoreProperty]
        public string PhyO_AlternateNo { get; set; }
        [FirestoreProperty]
        public string PhyO_MobileNo { get; set; }
        [FirestoreProperty]
        public string PhyO_Helpline { get; set; }
        [FirestoreProperty]
        public string PhyO_Website_URL { get; set; }
        [FirestoreProperty]
        public string PhyO_Surgery_Center_ID { get; set; }
        [FirestoreProperty]
        public string PhyO_Created_By { get; set; }
        [FirestoreProperty]
        public string PhyO_Created_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime PhyO_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime PhyO_Modify_Date { get; set; }
        [FirestoreProperty]
        public Boolean PhyO_Is_Active { get; set; }
        [FirestoreProperty]
        public Boolean PhyO_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string PhyO_User_Name { get; set; }
        [FirestoreProperty]
        public string PhyO_TimeZone { get; set; }
        [FirestoreProperty]
        public string[] PhyO_Specilities { get; set; }
        [FirestoreProperty]
        public List<MT_Specilities> PhyO_SpecilitiesList { get; set; }
        [FirestoreProperty]
        public List<MT_PhyO_Contact_Setting> PhyO_ContactSetting { get; set; }
        [FirestoreProperty]
        public SiteURL PhyO_SiteURL { get; set; }
        [FirestoreProperty]
        public Appearance PhyO_Appearance { get; set; }
        [FirestoreProperty]
        public Logo PhyO_Logo { get; set; }
        [FirestoreProperty]
        public Slider PhyO_Slider { get; set; }
        [FirestoreProperty]
        public List<Slider> PhyO_SliderList { get; set; }
        [FirestoreProperty]
        public Footer PhyO_Footer { get; set; }
        [FirestoreProperty]
        public Miscellaneous PhyO_Miscellaneous { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }
    }

    public class PhysicianOfficeResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Physician_Office Data { get; set; }
        public List<MT_Physician_Office> DataList { get; set; }
    }
}