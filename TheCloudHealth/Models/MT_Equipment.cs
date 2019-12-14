using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Cloud.Firestore;

namespace TheCloudHealth.Models
{
    [FirestoreData]
    public class MT_Equipment
    {
        [FirestoreProperty]
        public string Equip_Unique_ID { get; set; }
        [FirestoreProperty]
        public string Equip_Type { get; set; }
        [FirestoreProperty]
        public string Equip_Name { get; set; }
        [FirestoreProperty]
        public string Equip_Description { get; set; }
        [FirestoreProperty]
        public Boolean Equip_Is_Active { get; set; }
        [FirestoreProperty]
        public string Equip_Create_By { get; set; }
        [FirestoreProperty]
        public string Equip_CB_Name { get; set; }
        [FirestoreProperty]
        public string Equip_Create_By_Type { get; set; }
        [FirestoreProperty]
        public DateTime Equip_Create_Date { get; set; }
        [FirestoreProperty]
        public DateTime Equip_Modify_Date { get; set; }
        [FirestoreProperty]
        public string  Equip_Surgery_Physician_Id { get; set; }
        [FirestoreProperty]
        public string Equip_Office_Type { get; set; }
        [FirestoreProperty]
        public Boolean Equip_Is_Deleted { get; set; }
        [FirestoreProperty]
        public string Equip_User_Name { get; set; }
        [FirestoreProperty]
        public string Equip_TimeZone { get; set; }
        [FirestoreProperty]
        public string Slug { get; set; }

    }

    public class EquipmentResponse
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public MT_Equipment Data { get; set; }
        public List<MT_Equipment> DataList { get; set; }
    }
}